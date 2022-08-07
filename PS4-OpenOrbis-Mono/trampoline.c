#include <orbis/libkernel.h>
#include <errno.h>
#include <sys/stat.h>
#include <stdio.h>
#include "memory.h"
#include "imports.h"
#include "io.h"


//stolen from https://github.com/OSM-Made/Mono-Test/blob/af84e1dec5f02612bfc3f4634a4cc8f2474e5a1b/MonoTest/Detour.cpp
void WriteJump(void* Address, void* Destination){
	uint8_t JumpInstructions[] = {
	   0xFF, 0x25, 0x00, 0x00, 0x00, 0x00, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, // jmp QWORD PTR[Address]
	};

	//Write the address of our hook to the instruction.
	*(uint64_t*)(JumpInstructions + 6) = (uint64_t)Destination;

	sceKernelMprotect((void*)Address, sizeof(JumpInstructions), VM_PROT_ALL);
	memcpy(Address, JumpInstructions, sizeof(JumpInstructions));
}

U64 align(U64 x, U64 align) {
    return (x + align - 1) & ~(align - 1);
}

void* hookLoadSprxAssembly(const char* AssemblyName, int* OpenStatus, int UnkBool, int RefOnly) {
    klog("hook called...");
    klogf("Loading Assembly: %s", AssemblyName);

    void* fp = fopen(AssemblyName, "r");
    if (fp == 0)  {
      klog("Error opening file");
      return 0;
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
