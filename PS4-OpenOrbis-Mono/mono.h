#include <orbis/libkernel.h>
#include <dirent.h>

extern void (*mono_set_dirs)(const char* lib, const char* etc);
extern void* (*mono_jit_init)(const char* domain_name);
extern void* (*mono_init_from_assembly)(const char* domain_name, const char* filename);
extern void* (*mono_get_root_domain)(void);
extern void* (*mono_domain_assembly_open)(void* domain, const char *assembly_path);
extern void* (*mono_class_from_name)(void* image, const char* class_namespace, const char* class_name);
extern void* (*mono_class_get_method_from_name)(void* method_class, const char *method_name, int param_count);
extern void* (*mono_runtime_invoke)(void* method, void* obj, void** params, void** exc);
extern void* (*mono_jit_cleanup)(void* domain);
extern void* (*mono_thread_attach)(void* domain);
extern void* (*mono_assembly_get_image)(void* assembly);

extern int   (*mono_jit_exec)(void* domain, void* assembly, int argc, char* argv[]);
extern void* (*mono_assembly_load_from_full)(void* image, int* fname, int* status, int refonly);
extern void* (*mono_add_internal_call)(const char* methodPath, void* func);
extern void (*mono_debugger_agent_parse_options)(char* debug_options);

extern void (*mono_debug_init)(int debug_format);
extern void (*mono_debug_domain_create)(void* domain);
extern void (*mono_jit_parse_options)(int arc, char* argv[]);
extern void (*mono_debug_open_image_from_memory)(void* image, const char* raw_contents, int size);
extern void* (*mono_image_open_from_data_with_name)(char *data, unsigned int data_len, int need_copy, int* status, int refonly, const char *name);

extern int (*JitCreateSharedMemory)(int flags, size_t size, int protection, void** destinationHandle);
extern int (*JitCreateAliasOfSharedMemory)(int handle, int protection, void** destinationHandle);
extern int (*JitMapSharedMemory)(int handle, int protection, void **destination);

extern uint32_t (*sceKernelLoadStartModule_sys)(const char *, size_t, const void *, uint32_t, void *, void *);

extern void* (*sceKernelLoadStartModuleInternalForMono)(const char *moduleFileName, size_t args, const void *argp, uint32_t flags, void* pOpt, int *pRes);

typedef long long unsigned int U64;
#define countof(_array) (sizeof(_array) / sizeof(_array[0]))
