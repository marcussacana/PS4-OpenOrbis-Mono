#include "jailbreak_man.h"
#include "module.h"

#define EXPORT __attribute((visibility("default"))) __attribute__((used))

EXPORT void* sceGetModuleBase = get_module_base;
EXPORT void* sceJailbreak = jailbreak;
EXPORT void* sceUnjailbreak = unjailbreak;
EXPORT void* sceIsJailbroken = isJailbroken;
