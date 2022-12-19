#include <orbis/libkernel.h>
#include <dirent.h>

static void (*mono_set_dirs)(const char* lib, const char* etc);
static void* (*mono_jit_init)(const char* domain_name);
static void* (*mono_init_from_assembly)(const char* domain_name, const char* filename);
static void* (*mono_get_root_domain)(void);
static void* (*mono_domain_assembly_open)(void* domain, const char *assembly_path);
static void* (*mono_class_from_name)(void* image, const char* class_namespace, const char* class_name);
static void* (*mono_class_get_method_from_name)(void* method_class, const char *method_name, int param_count);
static void* (*mono_runtime_invoke)(void* method, void* obj, void** params, void** exc);
static void* (*mono_jit_cleanup)(void* domain);
static void* (*mono_thread_attach)(void* domain);
static void* (*mono_assembly_get_image)(void* assembly);

static int   (*mono_jit_exec)(void* domain, void* assembly, int argc, char* argv[]);
static void* (*mono_assembly_load_from_full)(void* image, int* fname, int* status, int refonly);
static void* (*mono_add_internal_call)(const char* methodPath, void* func);
static void (*mono_debugger_agent_parse_options)(char* debug_options);

static void (*mono_debug_init)(int debug_format);
static void (*mono_debug_domain_create)(void* domain);
static void (*mono_jit_parse_options)(int arc, char* argv[]);
static void (*mono_debug_open_image_from_memory)(void* image, const char* raw_contents, int size);
static void* (*mono_image_open_from_data_with_name)(char *data, unsigned int data_len, int need_copy, int* status, int refonly, const char *name);

static int (*JitCreateSharedMemory)(int flags, size_t size, int protection, void** destinationHandle);
static int (*JitCreateAliasOfSharedMemory)(int handle, int protection, void** destinationHandle);
static int (*JitMapSharedMemory)(int handle, int protection, void **destination);

static void* (*sceKernelLoadStartModuleInternalForMono)(const char *moduleFileName, size_t args, const void *argp, uint32_t flags, void* pOpt, int *pRes);

typedef long long unsigned int U64;
#define countof(_array) (sizeof(_array) / sizeof(_array[0]))
