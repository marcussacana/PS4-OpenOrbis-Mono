void WriteJump(void* AddressToHook, void* HookAddress);
void* hookLoadSprxAssembly(const char* AssemblyName, int* OpenStatus, int UnkBool, int RefOnly);
void InstallHooks();