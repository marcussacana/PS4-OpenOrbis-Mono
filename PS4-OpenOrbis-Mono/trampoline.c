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

//The KernelLoadStartModule export is just a check if you didn't set
//flags arguments, if not, it jump for the real function,
//we will use that to be able to keep the hook allways enabled,
//by calling the real LoadStartModule function directly
void* FindInternalFunction(void* Address){
	
	//Since is just reading maybe this isn't required, but in any case, better be sure.
    sceKernelMprotect((void*)Address, 0x20, PROT_READ | PROT_WRITE | PROT_EXEC);
	
	for (int i = 0; i < 0x20; i++){
		uint32_t* pDWORD = (uint32_t*)(Address+i);
		
		//find the function end (end with NOPs)
		if (*pDWORD == 0x90909090){
			
			//The last instruction is a jmp to the real LoadStartModule function
			uint8_t* pJmp = ((uint8_t*)pDWORD) - 5;
			
			if (*pJmp != 0xE9)
				return 0;//Whatever if fails, we still can do the dirty way disabling the hook temporally
			
			//Get the Jmp Offset
			pDWORD = (uint32_t*)(pJmp+1);
			
			//Calculate the jump offset
			return pJmp + 5 + *pDWORD;
		}
	}
	return 0;
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

    for (int i = len - 1; i > 0; i--) {
        if (path[i] == '\\' || path[i] == '//' || path[i] == '/') {
            return path + i + 1;
        }
    }

    return path;
}

char* extract_extension(char* path)
{
    int len = strlen(path);

    for (int i = len - 1; i > 0; i--) {
        if (path[i] == '.') {
            return path + i;
        }
    }

    return 0;
}

#ifdef DEBUG
#define LOG LOG
#define LOGF LOGf
#else
#define LOG(x)
#define LOGF(x,...)
#endif

void* hookLoadSprxAssembly(const char* AssemblyName, int* OpenStatus, int UnkBool, int RefOnly)
{
    LOGF("Loading Assembly: %s", AssemblyName);

    char* finalPath = AssemblyName;

    void* fp = fopen(AssemblyName, "r");
    if (fp == 0) {
        LOG("Error opening file");

        char hintPath[0x300] = "\x0";
        char* fname = extract_file_name(AssemblyName);

        const char* hints[] = { 
            "%s/mono/4.5/%s",
            "%s/mono/4.0/%s",
            "%s/mono/3.5/%s",
            "%s/mono/3.0/%s",
            "%s/mono/2.0/%s",
            "%s/mono/1.0/%s",
            "%s/mono/%s",
            "%s/%s"
        };

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
            LOG("No valid hints");
            return 0;
        }

        finalPath = hintPath;
        LOGF("Hint path matched: %s", hintPath);
    }
		
    fseek(fp, 0, SEEK_END);
    long int size = ftell(fp);
    fseek(fp, 0, SEEK_SET);

    char* data = malloc(size);
    int readed = fread(data, 1, size, fp);

    fclose(fp);

    if (readed != size) {
        LOG("Error reading the file");
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

            LOGF("Debug symbols loaded: %s", pdbPath);
        }
    }
#endif

    if (OpenStatus != 0)* OpenStatus = status;
	
	LOGF("Assembly Image: %x", Image);

    return Image;
}

char OriData[sizeof(JumpInstructions)];

typedef (*SceKernelLoadStartModuleInternal)(const char* path, size_t args, const void* argp, uint32_t flags, void* option, void* status);

SceKernelLoadStartModuleInternal sceKernelLoadStartModuleMod;

uint32_t hookSceKernelLoadStartModule(const char* path, size_t args, const void* argp, uint32_t flags, void* option, void* status)
{
    LOGF("sceKernelLoadStartModule: %s", path);
    uint32_t rst = sceKernelLoadStartModuleMod(path, args, argp, flags, option, status);
    
    if (rst & 0x80000000) {
        char altPath[0x300] = "\x0";
        remove_extension(path, altPath);

        char* fname = extract_file_name(altPath);
        char* orifname = extract_file_name(path);
		char* extension = extract_extension(fname);
		
		uint8_t IsAssembly = extension != 0 && ((strcmp(extension, ".dll") == 0) || (strcmp(extension, ".dll") == 0));
			
		char* RootDir = "/app0";
		
		
		if (jailbroken){
			char tmp[0x300];
			sprintf(&tmp, "%s/app0", appRoot);
			RootDir = tmp;
		}
		
        char hintPath[0x300] = "\x0";

        char* hints[] = { 
            "%s/sce_module/%s",
            "%s/%s",
            "%s/mono/4.5/%s",
            "%s/mono/4.5/%s.sprx"
        };
		
		int HintsCount = countof(hints);

        for (int i = 0; i < HintsCount; i++) {
            sprintf(&hintPath, hints[i], RootDir, fname);
            rst = sceKernelLoadStartModuleMod(hintPath, args, argp, flags, option, status);
            if (rst < 0x80000000)
                break;
			
            sprintf(&hintPath, hints[i], RootDir, orifname);
            rst = sceKernelLoadStartModuleMod(hintPath, args, argp, flags, option, status);
            if (rst < 0x80000000)
                break;
			
            sprintf(&hintPath, hints[i], "/app0", fname);
            rst = sceKernelLoadStartModuleMod(hintPath, args, argp, flags, option, status);
            if (rst < 0x80000000)
                break;
			
            sprintf(&hintPath, hints[i], "/app0", orifname);
            rst = sceKernelLoadStartModuleMod(hintPath, args, argp, flags, option, status);
            if (rst < 0x80000000)
                break;
        }
        
        if (rst & 0x80000000)
        {
            char* MountID = sceKernelGetFsSandboxRandomWord();
            
            char hintA[0x300] = "\x0";
            char hintB[0x300] = "\x0";
            
            sprintf(&hintA, "%s/%s/common/lib/%%s", appRoot, MountID);
            sprintf(&hintB, "/%s/common/lib/%%s", MountID);
            
            hints[0] = hintA;
            hints[1] = "/system/common/lib/%s";
			
			//Hints[2] and Hints[3] append the .sprx extension,
			//we are ignoring if the original extension is a dll,
			//if not, the mono can load a different sprx of the /mono/4.5 directory
            hints[2] = hintB;
            hints[3] = "/system/common/lib/%s.sprx";

            for (int i = 0; i < HintsCount; i++) {
            	sprintf(&hintPath, hints[i], fname);
                rst = sceKernelLoadStartModuleMod(hintPath, args, argp, flags, option, status);
                if (rst < 0x80000000)
                    break;
			
            	sprintf(&hintPath, hints[i], orifname);
                rst = sceKernelLoadStartModuleMod(hintPath, args, argp, flags, option, status);
                if (rst < 0x80000000)
                    break;
            }
        }
		
        if (rst < 0x80000000)
            LOGF("Hint path matched: %s", hintPath);
    } else {
        LOG("Original Path Loaded");
    }


    return rst;
}

uint32_t SceKernelLoadStartModuleAlt(const char* path, size_t args, const void* argp, uint32_t flags, void* option, void* status){
    int notJailbroken = 0;
    if (!jailbroken) {
		klog("[WARN] SceKernelLoadStartModuleAlt called without Jailbreak");
        notJailbroken = 1;
        jailbreak(0); //Since we need diable the hook temporally, we need jailbreak to patch the memory.
    }
	
	//disable hook - lazy to do a proper hooking
    sceKernelMprotect((void*)sceKernelLoadStartModule, sizeof(JumpInstructions), PROT_READ | PROT_WRITE | PROT_EXEC);
    memcpy(sceKernelLoadStartModule, OriData, sizeof(JumpInstructions)); 
	
	uint32_t rst = sceKernelLoadStartModule(path, args, argp, flags, option, status);
	
	//re-enable hook
    WriteJump(sceKernelLoadStartModule, hookSceKernelLoadStartModule, 0); 
	
    if (notJailbroken) {
        unjailbreak(1);
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
	
	sceKernelLoadStartModuleMod = (SceKernelLoadStartModuleInternal)FindInternalFunction(sceKernelLoadStartModule);
	
	if (sceKernelLoadStartModuleMod){
		LOGF("SceKernelLoadStartModuleInternal successfully loaded");
	} else {
		LOGF("SceKernelLoadStartModuleInternal failed");
		sceKernelLoadStartModuleMod = SceKernelLoadStartModuleAlt;
	}
	
	//This hook has made for the DLLImport search in more paths before fail
	WriteJump(sceKernelLoadStartModule, hookSceKernelLoadStartModule, &OriData);
	
    //MUST UPDATE
    //6.72: 0x18CC60
    void* AllocJIT = ((void*)MonoAddr) + 0x18CC60;
    WriteJump(AllocJIT, hookLoadSprxAssembly, 0);
}
