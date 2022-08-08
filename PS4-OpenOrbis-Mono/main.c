#include <stddef.h>
#include <stdlib.h>
#include <orbis/SystemService.h>
#include "ps4-libjbc/jailbreak.h"
#include "trampoline.h"
#include "mono.h"
#include "io.h"
#include "SDL2.h"

struct jbc_cred OriCred;

int jailbroken = 0;
 
int jailbreak()
{
    if (jailbroken)
        return 0;

    struct jbc_cred cr;
    if(jbc_get_cred(&cr))
        return -1;
    if(jbc_get_cred(&OriCred))
        return -1;
    if(jbc_jailbreak_cred(&cr))
        return -1;
    //cr.jdir = 0;
    //cr.sceProcType = 0x3800000000000010;
    //cr.sonyCred = 0x40001c0000000000;
    //cr.sceProcCap = 0x900000000000ff00;
    if(jbc_set_cred(&cr))
        return -1;

    jailbroken = 1;
    return 0;
}

int unjailbreak(){
    if (!jailbroken)
        return 0;
    if(jbc_set_cred(&OriCred))
        return -1;
    jailbroken = 0;
    return 0;
}

void* startMono(){

    klog("Starting Mono...");

    auto domain = mono_get_root_domain();
    if (!domain){
    	mono_set_dirs(baseDir, baseCon);
    	//domain = mono_init_from_assembly("main", mainLib);
        domain = mono_jit_init("main");
    }
    
    if (!domain){
        klog("Failed to init the mono domain");
    	return 0;
    }
    
    klog("Mono domain Initialized");
    return domain;
}

const char* JailedBase = "/app0";

char* getBaseDirectory(){
    if (jailbroken)
        return baseDir;
    return JailedBase;
}

void runMain()
{
    auto rootDomain = mono_get_root_domain();
    if (!rootDomain){
        klog("get_root_domain failed");
        return;
    }

    klog("img open");
    void* mainImage = hookLoadSprxAssembly(mainExe, 0, 0, 0);

    klog("asm open");
    void* mainAssembly = mono_assembly_load_from_full(mainImage, mainExe, 0, 0);
    if (!mainAssembly){
        klog("Failed to get the main assembly");
        return;
    }

    klogf("mainAssembly: %x", mainAssembly);

    void* programClass = mono_class_from_name(mainImage, "Orbis", "Program");
    
    if (!programClass){
        klog("Get program class Failed");
        return;
    }
    
    klog("adding kernel internal calls...");
    mono_add_internal_call("Orbis.Internals.Kernel::Log(void*)", klog);
    mono_add_internal_call("Orbis.Internals.Kernel::malloc(int)", malloc);
    mono_add_internal_call("Orbis.Internals.Kernel::free(void*)", free);
    mono_add_internal_call("Orbis.Internals.Kernel::Jailbreak", jailbreak);
    mono_add_internal_call("Orbis.Internals.Kernel::Unjailbreak", unjailbreak);
    klog("adding IO internal calls...");
    mono_add_internal_call("Orbis.Internals.IO::GetBaseDirectory", getBaseDirectory);
    klog("adding SDL internal calls...");
    SetSDLInternals();
    klog("internal calls added");

    void* methodMain = mono_class_get_method_from_name(programClass, "Main", 0);

    if (!methodMain){
        klog("Failed to find Orbis.Program.Main() Method");
        return;
    }

    //Load freetype
    sceSysmoduleLoadModule(0x009A);

    klog("Starting program...");
    char* argv[] = { 0 };
    mono_runtime_invoke(methodMain, 0, argv, 0);

}

void run(){
    void* rootDomain = startMono();
    if (rootDomain == 0)
        return;

    runMain();
    sceSystemServiceLoadExec("exit", NULL);
}

int main()
{
    jailbreak();

    findAppMount(&appRoot);

    sprintf(&baseDir, "%s/app0", appRoot);
    sprintf(&mainExe, "%s/main.exe", baseDir);
    sprintf(&baseCon, "%s/mono", baseDir);

    char monoLib[0x100] = "\x0";
    sprintf(&monoLib, "%s/sce_module/libmonosgen-2.0.prx", baseDir);
    
    auto mono_framework = sceKernelLoadStartModule(monoLib, 0, NULL, 0, 0, 0);
    auto libkernel_sys = sceKernelLoadStartModule("/system/common/lib/libkernel_sys.sprx", 0, NULL, 0, 0, 0);
    auto libKernel = sceKernelLoadStartModule("libkernel.sprx", 0, NULL, 0, 0, 0);
    
    if (mono_framework == -1){
	    klog("DNP: Failed o Load the Mono\n");
    	return -1;
    }
    if (libkernel_sys == -1){
	    klog("DNP: Failed o Load the libkernel_sys\n");
    	return -1;
    }
    if (libKernel == -1){
	    klog("DNP: Failed o Load the libKernel\n");
    	return -1;
    }

    sceKernelDlsym(mono_framework, "mono_set_dirs", (void**)&mono_set_dirs);
    sceKernelDlsym(mono_framework, "mono_jit_init", (void**)&mono_jit_init);
    sceKernelDlsym(mono_framework, "mono_init_from_assembly", (void**)&mono_init_from_assembly);
    sceKernelDlsym(mono_framework, "mono_get_root_domain", (void**)&mono_get_root_domain);
    sceKernelDlsym(mono_framework, "mono_domain_assembly_open", (void**)&mono_domain_assembly_open);
    sceKernelDlsym(mono_framework, "mono_class_from_name", (void**)&mono_class_from_name);
    sceKernelDlsym(mono_framework, "mono_class_get_method_from_name", (void**)&mono_class_get_method_from_name);
    sceKernelDlsym(mono_framework, "mono_runtime_invoke", (void**)&mono_runtime_invoke);
    sceKernelDlsym(mono_framework, "mono_jit_cleanup", (void**)&mono_jit_cleanup);
    sceKernelDlsym(mono_framework, "mono_image_open_from_data_with_name", (void**)&mono_image_open_from_data_with_name);
    sceKernelDlsym(mono_framework, "mono_thread_attach", (void**)&mono_thread_attach);
    sceKernelDlsym(mono_framework, "mono_assembly_get_image", (void**)&mono_assembly_get_image);
    sceKernelDlsym(mono_framework, "mono_assembly_load_from_full", (void**)&mono_assembly_load_from_full);
    sceKernelDlsym(mono_framework, "mono_add_internal_call", (void**)&mono_add_internal_call);
    
    sceKernelDlsym(libKernel, "sceKernelJitCreateSharedMemory", (void**)&JitCreateSharedMemory);
    sceKernelDlsym(libKernel, "sceKernelJitCreateAliasOfSharedMemory", (void**)&JitCreateAliasOfSharedMemory);
    sceKernelDlsym(libKernel, "sceKernelJitMapSharedMemory", (void**)&JitMapSharedMemory);

    sceKernelDlsym(libkernel_sys, "sceKernelLoadStartModuleInternalForMono", (void**)&sceKernelLoadStartModuleInternalForMono);

    InstallHooks();

    sceSystemServiceHideSplashScreen();

/*
    int tid = 0;
    scePthreadCreate(&tid, 0, Run, 0, "MonoThread");

    while (1)
    {
        sceKernelUsleep(5 * 100000);
    }
*/
    
    run();

    return 0;
}
