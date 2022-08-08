#include <orbis/libkernel.h>
#include <dirent.h>

char appRoot[0x100] = "\x0";
char baseCon[0x100] = "\x0";
char baseDir[0x100] = "\x0";
char mainExe[0x100] = "\x0";

void klog(const char* str)
{
    char buff[0x300];
    sprintf(&buff, "%s\n", str);
    sceKernelDebugOutText(0, buff);
}

void klogf(const char* str, void* val)
{
    char buff[0x300];
    char buff2[0x300];
    sprintf(&buff, "%s\n", str);
    sprintf(&buff2, &buff, val);
    sceKernelDebugOutText(0, buff2);
}

int direxists(const char* path){
    void* dp = opendir(path);
    if (dp == 0)
        return 0;
    closedir(dp);
    return 1;
}

void findAppMount(char* path){
    void* dp;
    struct dirent *ep;

    char* MountID = sceKernelGetFsSandboxRandomWord();

    dp = opendir("/mnt/sandbox/");
    if (dp != 0)
    {
        while (ep = readdir(dp))
        {
            char sbPath[0x100];
            sprintf(&sbPath, "/mnt/sandbox/%s/%s", ep->d_name, MountID);

            if (!direxists(sbPath))
                continue;

            sprintf(path, "/mnt/sandbox/%s", ep->d_name);
            klogf("mount dir found: %s", path);
            break;
        }
      closedir(dp);
    }
}
