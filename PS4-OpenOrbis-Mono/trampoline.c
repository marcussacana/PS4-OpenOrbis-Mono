#include <orbis/libkernel.h>
#include <errno.h>
#include <sys/stat.h>
#include <stdio.h>
#include "memory.h"
#include "mono.h"
#include "io.h"
#include "jailbreak_man.h"

uint8_t JumpInstructions[] = {
    0xFF, 0x25, 0x00, 0x00, 0x00, 0x00, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, // jmp QWORD PTR[Address]
};

//stolen from https://github.com/OSM-Made/Mono-Test/blob/af84e1dec5f02612bfc3f4634a4cc8f2474e5a1b/MonoTest/Detour.cpp
void WriteJump(void* Address, void* Destination, char* OriInstructions)
{
    //Write the address of our hook to the instruction.
    *(uint64_t*)(JumpInstructions + 6) = (uint64_t)Destination;

    sceKernelMprotect((void*)Address, sizeof(JumpInstructions), PROT_READ | PROT_WRITE | PROT_EXEC);

    if (OriInstructions) {
        memcpy(OriInstructions, Address, sizeof(JumpInstructions));
    }

    memcpy(Address, JumpInstructions, sizeof(JumpInstructions));
}

void remove_extension(const char* path, char* new_path)
{
    int len = strlen(path);
    strcpy(new_path, path);
    for (int i = len - 1; i > 0; i--) {
        if (path[i] == '.') {
            new_path[i] = 0;
            return;
        }
    }
}

char* extract_file_name(char* path)
{
    int len = strlen(path);
    int flag = 0;

    for (int i = len - 1; i > 0; i--) {
        if (path[i] == '\\' || path[i] == '//' || path[i] == '/') {
            flag = 1;
            path = path + i + 1;
            break;
        }
    }

    return path;
}

void* hookLoadSprxAssembly(const char* AssemblyName, int* OpenStatus, int UnkBool, int RefOnly)
{
    klogf("Loading Assembly: %s", AssemblyName);

    char* finalPath = AssemblyName;

    void* fp = fopen(AssemblyName, "r");
    if (fp == 0) {
        klog("Error opening file");

        char hintPath[0x300] = "\x0";
        char* fname = extract_file_name(AssemblyName);

        const char* hints[] = { "%s/mono/4.5/%s",
            "%s/mono/4.0/%s",
            "%s/mono/3.5/%s",
            "%s/mono/3.0/%s",
            "%s/mono/2.0/%s",
            "%s/mono/1.0/%s",
            "%s/mono/%s",
            "%s/%s" };

        for (int i = 0; i < countof(hints); i++) {
            sprintf(&hintPath, hints[i], "/app0", fname);
            fp = fopen(hintPath, "r");
            if (fp != 0)
                break;

            sprintf(&hintPath, hints[i], baseDir, fname);
            fp = fopen(hintPath, "r");
            if (fp != 0)
                break;
        }

        if (fp == 0) {
            klog("No valid hints");
            return 0;
        }

        finalPath = hintPath;
        klogf("Hint path matched: %s", hintPath);
    }

    fseek(fp, 0, SEEK_END);
    long int size = ftell(fp);
    fseek(fp, 0, SEEK_SET);

    char* data = malloc(size);
    int readed = fread(data, 1, size, fp);

    fclose(fp);

    if (readed != size) {
        klog("Error reading the file");
        return 0;
    }

    int status = 0;
    void* Image = mono_image_open_from_data_with_name(data, size, 0, &status, RefOnly, AssemblyName);

#ifdef DEBUG
    if (Image)
    {
        char* noExtPath[0x300];
        char* pdbPath[0x300];

        remove_extension(finalPath, noExtPath);
        sprintf(pdbPath, "%s.pdb", noExtPath);

        fp = fopen(pdbPath, "r");

        if (fp) {
            fseek(fp, 0, SEEK_END);
            size = ftell(fp);
            fseek(fp, 0, SEEK_SET);

            char* pdb = malloc(size);
            int readed = fread(pdb, 1, size, fp);
            fclose(fp);

            mono_debug_open_image_from_memory(Image, pdb, size);

            klogf("Debug symbols loaded: %s", pdbPath);
        }
    }
    #endif

        if (OpenStatus != 0)* OpenStatus = status;

    return Image;
}

char OriData[sizeof(JumpInstructions)];

uint32_t hookSceKernelLoadStartModule(const char* path, size_t args, const void* argp, uint32_t flags, void* option, void* status)
{
    int notJailbroken = 0;
    if (!jailbroken) {
        notJailbroken = 1;
        jailbreak(0); //Since we need diable the hook temporally, we need jailbreak to patch the memory.
    }

    sceKernelMprotect((void*)sceKernelLoadStartModule, sizeof(JumpInstructions), PROT_READ | PROT_WRITE | PROT_EXEC);
    memcpy(sceKernelLoadStartModule, OriData, sizeof(JumpInstructions)); //disable hook - lazy to do a proper hooking

    klogf("sceKernelLoadStartModule: %s", path);
    auto rst = sceKernelLoadStartModule(path, args, argp, flags, option, status);
    if (rst & 0x80000000) {
        char altPath[0x300] = "\x0";
        remove_extension(path, altPath);

        char* fname = extract_file_name(altPath);
        char* orifname = extract_file_name(path);

        char hintPath[0x300] = "\x0";
        const char* hints[] = { "%s/sce_module/%s",
            "%s/%s" };

        for (int i = 0; i < countof(hints); i++) {
            sprintf(&hintPath, hints[i], baseDir, fname);
            rst = sceKernelLoadStartModule(hintPath, args, argp, flags, option, status);
            if (rst & 0x80000000 == 0)
                break;

            sprintf(&hintPath, hints[i], "/app0", fname);
            rst = sceKernelLoadStartModule(hintPath, args, argp, flags, option, status);
            if (rst & 0x80000000 == 0)
                break;

            sprintf(&hintPath, hints[i], baseDir, orifname);
            rst = sceKernelLoadStartModule(hintPath, args, argp, flags, option, status);
            if (rst & 0x80000000 == 0)
                break;

            sprintf(&hintPath, hints[i], "/app0", orifname);
            rst = sceKernelLoadStartModule(hintPath, args, argp, flags, option, status);
            if (rst & 0x80000000 == 0)
                break;
        }

        if (rst & 0x80000000 == 0)
            klogf("Hint path matched: %s", hintPath);
    }

    WriteJump(sceKernelLoadStartModule, hookSceKernelLoadStartModule, 0); //re-enable hook

    if (notJailbroken) {
        unjailbreak();
    }

    return rst;
}

void InstallHooks()
{
    klog("Installing hooks...");

    U64 MonoAddr = 0;
    U64 MonoSize = 0;
    get_module_base("libmonosgen-2.0.prx", &MonoAddr, &MonoSize);

    if (MonoAddr == 0) {
        klog("Failed to get libmonosgen-2.0.sprx address");
    }

    //This hook has made for the DLLImport search in more paths before fail
    WriteJump(sceKernelLoadStartModule, hookSceKernelLoadStartModule, &OriData);

    //MUST UPDATE
    //6.72: 0x18CC60
    void* AllocJIT = MonoAddr + 0x18CC60;
    WriteJump(AllocJIT, hookLoadSprxAssembly, 0);
}
