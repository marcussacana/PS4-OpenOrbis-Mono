#include <stddef.h>
#include <stdlib.h>
#include <orbis/SystemService.h>
#include "trampoline.h"
#include "mono.h"
#include "io.h"
#include "jailbreak_man.h"

void (*mono_set_dirs)(const char* lib, const char* etc);
void* (*mono_jit_init)(const char* domain_name);
void* (*mono_init_from_assembly)(const char* domain_name, const char* filename);
void* (*mono_get_root_domain)(void);
void* (*mono_domain_assembly_open)(void* domain, const char *assembly_path);
void* (*mono_class_from_name)(void* image, const char* class_namespace, const char* class_name);
void* (*mono_class_get_method_from_name)(void* method_class, const char *method_name, int param_count);
void* (*mono_runtime_invoke)(void* method, void* obj, void** params, void** exc);
void* (*mono_jit_cleanup)(void* domain);
void* (*mono_thread_attach)(void* domain);
void* (*mono_assembly_get_image)(void* assembly);

int   (*mono_jit_exec)(void* domain, void* assembly, int argc, char* argv[]);
void* (*mono_assembly_load_from_full)(void* image, int* fname, int* status, int refonly);
void* (*mono_add_internal_call)(const char* methodPath, void* func);
void (*mono_debugger_agent_parse_options)(char* debug_options);

void (*mono_debug_init)(int debug_format);
void (*mono_debug_domain_create)(void* domain);
void (*mono_jit_parse_options)(int arc, char* argv[]);
void (*mono_debug_open_image_from_memory)(void* image, const char* raw_contents, int size);
void* (*mono_image_open_from_data_with_name)(char *data, unsigned int data_len, int need_copy, int* status, int refonly, const char *name);

int (*JitCreateSharedMemory)(int flags, size_t size, int protection, void** destinationHandle);
int (*JitCreateAliasOfSharedMemory)(int handle, int protection, void** destinationHandle);
int (*JitMapSharedMemory)(int handle, int protection, void **destination);

void* (*sceKernelLoadStartModuleInternalForMono)(const char *moduleFileName, size_t args, const void *argp, uint32_t flags, void* pOpt, int *pRes);


static int def_jailbreak()
{
    return jailbreak(0);
}

void* startMono()
{

#ifdef DEBUG
    klog("Initializing Debugger...");

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

const char* JailedBase = "/app0";

char* getBaseDirectory()
{
    if (jailbroken)
        return baseDir;
    return JailedBase;
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

    klog("Adding kernel internal calls...");
    mono_add_internal_call("Orbis.Internals.Kernel::Log(void*)", klog);
    mono_add_internal_call("Orbis.Internals.Kernel::malloc(int)", malloc);
    mono_add_internal_call("Orbis.Internals.Kernel::free(void*)", free);
    mono_add_internal_call("Orbis.Internals.Kernel::Jailbreak", jailbreak);
    mono_add_internal_call("Orbis.Internals.Kernel::JailbreakCred(long)", def_jailbreak);
    mono_add_internal_call("Orbis.Internals.Kernel::Unjailbreak", unjailbreak);
    klog("Adding IO internal calls...");
    mono_add_internal_call("Orbis.Internals.IO::GetBaseDirectory", getBaseDirectory);
    klog("Internal calls added.");

    void* methodMain = mono_class_get_method_from_name(programClass, "Main", 0);

    if (!methodMain) {
        klog("Failed to find Orbis.Program.Main() Method");
        return;
    }

    //Load freetype
    sceSysmoduleLoadModule(0x009A);

//#ifndef DEBUG
//    unjailbreak();
//#endif

    klog("Starting program...");
    char* argv[] = { 0 };
    mono_runtime_invoke(methodMain, 0, argv, 0);
}

void run()
{
    void* rootDomain = startMono();
    if (rootDomain == 0)
        return;

    sceSystemServiceHideSplashScreen();
    runMain();
    sceSystemServiceLoadExec("exit", NULL);
}

int main()
{
    def_jailbreak();

    findAppMount(&appRoot);

    sprintf(&baseDir, "%s/app0", appRoot);
    sprintf(&mainExe, "%s/main.exe", baseDir);
    sprintf(&baseCon, "%s/mono", baseDir);

    char pkgLib[0x100] = "\x0";
    sprintf(&pkgLib, "%s/sce_module/libmonosgen-2.0.prx", baseDir);

    auto mono_framework = sceKernelLoadStartModule(pkgLib, 0, NULL, 0, 0, 0);
    auto libkernel_sys = sceKernelLoadStartModule("/system/common/lib/libkernel_sys.sprx", 0, NULL, 0, 0, 0);
    auto libKernel = sceKernelLoadStartModule("libkernel.sprx", 0, NULL, 0, 0, 0);

    if (mono_framework & 0x80000000) {
        klog("Failed o Load the Mono\n");
        return -1;
    }
    if (libkernel_sys & 0x80000000) {
        klog("Failed o Load the libkernel_sys\n");
        return -1;
    }
    if (libKernel & 0x80000000) {
        klog("Failed o Load the libKernel\n");
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
    sceKernelDlsym(libKernel, "sceKernelJitMapSharedMemory", (void**)&JitMapSharedMemory);

    sceKernelDlsym(libkernel_sys, "sceKernelLoadStartModuleInternalForMono", (void**)&sceKernelLoadStartModuleInternalForMono);

    InstallHooks();

    run();

    return 0;
}
