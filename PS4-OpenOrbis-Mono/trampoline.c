#include <orbis/libkernel.h>
#include <errno.h>
#include <sys/stat.h>
#include <stdio.h>
#include "memory.h"
#include "mono.h"
#include "io.h"


//stolen from https://github.com/OSM-Made/Mono-Test/blob/af84e1dec5f02612bfc3f4634a4cc8f2474e5a1b/MonoTest/Detour.cpp
void WriteJump(void* Address, void* Destination){
	uint8_t JumpInstructions[] = {
	   0xFF, 0x25, 0x00, 0x00, 0x00, 0x00, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, // jmp QWORD PTR[Address]
	};

	//Write the address of our hook to the instruction.
	*(uint64_t*)(JumpInstructions + 6) = (uint64_t)Destination;

	sceKernelMprotect((void*)Address, sizeof(JumpInstructions), PROT_READ | PROT_WRITE | PROT_EXEC);
	memcpy(Address, JumpInstructions, sizeof(JumpInstructions));
}

char* extract_file_name(char *path)
{
    int len = strlen(path);
    int flag=0;

    
    for(int i=len-1; i>0; i--)
    {
        if(path[i]=='\\' || path[i]=='//' || path[i]=='/' )
        {
            flag=1;
            path = path+i+1;
            break;
        }
    }
    return path;
}

void* hookLoadSprxAssembly(const char* AssemblyName, int* OpenStatus, int UnkBool, int RefOnly) {
    klog("hook called...");
    klogf("Loading Assembly: %s", AssemblyName);

    void* fp = fopen(AssemblyName, "r");
    if (fp == 0)  {
        klog("Error opening file");
        
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
        
        for (int i = 0; i < countof(hints); i++)
        {
            sprintf(&hintPath, hints[i], baseDir, fname);
            fp = fopen(hintPath, "r");
            if (fp != 0)
                break;
            
            sprintf(&hintPath, hints[i], "/app0", fname);
            fp = fopen(hintPath, "r");
            if (fp != 0)
                break;
        }

        
        if (fp == 0){
            klog("No valid hint");
            return 0;
        }
        
        klogf("Hint path matched: %s", hintPath);
    }

    klogf("FHandle: 0x%x", fp);
    
    fseek(fp, 0, SEEK_END);
    long int size = ftell(fp);
    fseek(fp, 0, SEEK_SET);

    klogf("FSize: %d", size);

    
    char* data = malloc(size);
    if(data == MAP_FAILED) {
       klog("malloc failed");
      return 0;
    }

    int readed = fread(data, 1, size, fp);
    fclose(fp);

    if (readed != size){
      klog("Error reading the file");
      return 0;
    }

    int status = 0;
    void* Image = mono_image_open_from_data_with_name(data, size, 0, &status, RefOnly, AssemblyName);
    klogf("Image: %x", Image);
    klogf("Status: 0x%x", status);
    
    if (OpenStatus != 0)
        *OpenStatus = status;

    return Image;
}

void InstallHooks(){
    klog("installing hooks...");
    
    U64 MonoAddr = 0;
    U64 MonoSize = 0;
    get_module_base("libmonosgen-2.0.sprx", &MonoAddr, &MonoSize);
    
    if (MonoAddr == 0){
    	klog("Failedt to get libmonosgen-2.0.sprx address");
    }


    //MUST UPDATE
    //6.72: 0X18CC60
    void* AllocJIT = MonoAddr + 0x18CC60;
    WriteJump(AllocJIT, hookLoadSprxAssembly);
}
