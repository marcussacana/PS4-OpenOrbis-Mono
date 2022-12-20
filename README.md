# PS4-OpenOrbis-Mono
 A Sample Mono Entrypoint for PS4 homebrews with SDL2 working

If you want, you can use the pre-built binaries and just rebuild the "main" C# project,  
and by replacing the main.exe inside the pkg and/or adding any new assemblies,
in the PKG, you will be able to create your HB even without deal with the OpenOrbis SDK.
 
### How to build (Linux)
 - Install the OpenOrbis SDK
 - Install the Mono SDK
 - Run `make`
 
### How to build (Windows)
 - Install and Build the OpenOrbis SDK
 - Install the Visual Studio
 - Install LLVM `choco install llvm`
 - Run the `Developer Command Prompt for VS`
 - Open this repo directory in the cmd
 - Run `build-windows.bat`

### How to debug 
 - Build the homebrew with `make debug` or `build-windows.bat debug`
 - Install and Start the homebrew (it will freeze, that's ok)
 - With Monodevelop or Jetbrains, connect the remote debugging at `2222`
 
### Notes
 - The Mono runtime needs to be in a jailbroken process to be able to use the PS4 JIT features.
 
### How it works
The PS4 default mono runtime allways try load the modules from sprx files,
in this example, at `/PS4-OpenOrbis-Mono/trampoline.c` a hook in the mono runtime is
installed in the function that try open the sprx assembly, and instead, load the .dll or .exe assembly
file and call the mono runtime export `mono_image_open_from_data_with_name`, because it was made
for load assembly direct from memory, that function don't expect a sprx file, but a common
assembly file instead, allowing we load our mono libraries.

Since it depends of hooks, we need or ship the mono runtime in the pkg with a known offset,
or instead detect the firmware version and find all possible offsets for all FWs.
While the first method is easy and safest and works with different FWs, you will share a sony binary, 
and that can be a license issue.  
In the second method you don't need to ship the PS4 mono runtime library, but instead you will
need to add offests for each FWs and be sure, when a new JB came you should need update the offsets as well.
This second method can works with different FWs as well, but that if you make the code detect the FW,
and use the correct hook offset.  
The good news about the second method is how is just a single hook, then you just need found one offset,
and the given function is the only one that references the string `%s.sprx`, this make the job to find the correct function easy.  
![image](https://user-images.githubusercontent.com/10576957/183532405-397dadce-a425-418e-a5e7-c935ca56d3bc.png)

A third way and correct way, should be create a tool to pack the assemblies as fake sprx, I didn't tried that.  
Really, I should try do that before create a hook, lol.

#### Tested in FW 6.72 and 9.00 only

### Special Thanks:
Lighting Mods, AlAzif, Bucanero, Flatz, sleirsgoevy, OSM-Made and of course, the OpenOrbis team.

