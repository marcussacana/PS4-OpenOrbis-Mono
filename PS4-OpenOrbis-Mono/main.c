#include <stddef.h>
#include <stdlib.h>
#include <orbis/SystemService.h>
#include <orbis/Sysmodule.h>
#include <stdio.h>
#include "main.h"

const char* JailedBase = "/app0";

char* getBaseDirectory()
{
    if (isJailbroken())
        return baseDir;
    return JailedBase;
}


void* startMono()
{

#ifndef DEBUG
    klog("Mono Shared Area Disabled");
    setenv("MONO_DISABLE_SHARED_AREA", "1", 1);
#endif

#ifdef DEBUG
    klog("Initializing Debugger at port 2222...");

    mono_debugger_agent_parse_options("address=0.0.0.0:2222,transport=dt_socket,server=y");
    mono_debug_init(1);

    const char* options[] = { "--soft-breakpoints" };

    mono_jit_parse_options(sizeof(options) / sizeof(char*), (char**)options);
#endif
	
    klog("Starting Mono...");

    void* domain = mono_get_root_domain();
	
    if (!domain) {
        mono_set_dirs(baseDir, baseCon);
        domain = mono_jit_init("main");
    }

    if (!domain) {
        klog("Failed to init the mono domain");
        return 0;
    }

#ifdef DEBUG
    mono_debug_domain_create(domain);
#endif

    klog("Mono domain Initialized");
    return domain;
}

void runMain()
{
    void* rootDomain = mono_get_root_domain();
    if (!rootDomain) {
        klog("get_root_domain failed");
        return;
    }

    void* mainImage = hookLoadSprxAssembly(mainExe, 0, 0, 0);
    void* mainAssembly = mono_assembly_load_from_full(mainImage, mainExe, 0, 0);
    
    if (!mainAssembly) {
        klog("Failed to get the main assembly");
        return;
    }

    klogf("Main Assembly: %x", mainAssembly);

    void* programClass = mono_class_from_name(mainImage, "Orbis", "Program");

    if (!programClass) {
        klog("Failed to find the class: Orbis.Program");
        return;
    }
	
	

    void* methodMain = mono_class_get_method_from_name(programClass, "Main", 0);

    if (!methodMain) {
        klog("Failed to find Orbis.Program.Main() Method");
        return;
    }

    //Load freetype
    sceSysmoduleLoadModule(0x009A);

    klog("Starting program...");
    char* argv[] = { 0 };
    mono_runtime_invoke(methodMain, 0, argv, 0);
}

void run()
{
    void* rootDomain = startMono();
    if (rootDomain == 0)
        return;
	
    runMain();
    sceSystemServiceLoadExec("exit", NULL);
}

int main()
{
	klog("Main Begin");
	
	char* sandboxWord = sceKernelGetFsSandboxRandomWord();
	
	char syslib[0x100] = "\x0";
	sprintf(&syslib, "/%s/common/lib/libkernel.sprx", sandboxWord);
	
    int libKernel = sceKernelLoadStartModule(syslib, 0, NULL, 0, 0, 0);	
	int libMonoUtil = sceKernelLoadStartModule("/app0/sce_module/libMonoUtils.sprx", 0, NULL, 0, 0, 0);
	
    if (libMonoUtil & 0x80000000) {
        klog("Failed o Load the MonoUtil Library");
        return -1;
    }	
    if (libKernel & 0x80000000) {
        klog("Failed o Load the libKernel");
        return -1;
    }
	
    sceKernelDlsym(libMonoUtil, "jailbreak", (void**)&jailbreak);
    sceKernelDlsym(libMonoUtil, "unjailbreak", (void**)&unjailbreak);
    sceKernelDlsym(libMonoUtil, "isJailbroken", (void**)&isJailbroken);	
    sceKernelDlsym(libMonoUtil, "get_module_base", (void**)&get_module_base);
	
    jailbreak(0);

    findAppMount(&appRoot);

    sprintf(&baseDir, "%s/app0", appRoot);
    sprintf(&mainExe, "%s/main.exe", baseDir);
    sprintf(&baseCon, "%s/mono", baseDir);
	
    char pkgLib[0x100] = "\x0";
    sprintf(&pkgLib, "%s/sce_module/libmonosgen-2.0.prx", baseDir);
	
	int libSceSystemService = sceKernelLoadStartModule("/system/common/lib/libSceSystemService.sprx", 0, NULL, 0, 0, 0);
	int libSceNet = sceKernelLoadStartModule("/system/common/lib/libSceNet.sprx", 0, NULL, 0, 0, 0);
    int mono_framework = sceKernelLoadStartModule(pkgLib, 0, NULL, 0, 0, 0);

	if (libSceNet & 0x80000000){
        klog("Failed o Load the libSceNet");
        return -1;		
	}
	if (libSceSystemService & 0x80000000){
        klog("Failed o Load the libSceSystemService");
        return -1;		
	}
    if (mono_framework & 0x80000000) {
        klog("Failed o Load the Mono");
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
    sceKernelDlsym(mono_framework, "mono_debugger_agent_parse_options", (void**)&mono_debugger_agent_parse_options);
    sceKernelDlsym(mono_framework, "mono_debug_init", (void**)&mono_debug_init);
    sceKernelDlsym(mono_framework, "mono_debug_domain_create", (void**)&mono_debug_domain_create);
    sceKernelDlsym(mono_framework, "mono_jit_parse_options", (void**)&mono_jit_parse_options);
    sceKernelDlsym(mono_framework, "mono_debug_open_image_from_memory", (void**)&mono_debug_open_image_from_memory);

    sceKernelDlsym(libKernel, "sceKernelJitCreateSharedMemory", (void**)&JitCreateSharedMemory);
    sceKernelDlsym(libKernel, "sceKernelJitCreateAliasOfSharedMemory", (void**)&JitCreateAliasOfSharedMemory);
    sceKernelDlsym(libKernel, "sceKernelJitMapSharedMemory", (void**)&JitCreateAliasOfSharedMemory);

    sceKernelDlsym(libKernel, "sceKernelLoadStartModule", (void**)&sceKernelLoadStartModule_sys);
	
    sceKernelDlsym(libKernel, "sceKernelLoadStartModuleInternalForMono", (void**)&sceKernelLoadStartModuleInternalForMono);
	
    InstallHooks();

    run();

    return 0;
}
