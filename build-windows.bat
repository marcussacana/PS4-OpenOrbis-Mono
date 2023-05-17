echo off
cls
SETLOCAL EnableDelayedExpansion

Rem Package information
set PKG_TITLE="OpenOrbis Mono Sample"
set PKG_VERSION="1.00"
set PKG_ASSETS="assets"
set PKG_TITLE_ID="MONO00001"
set PKG_CONTENT_ID="IV0000-MONO00001_00-MONOSAMPLE000000"

set sys_authinfo="000000000000000000000000001C004000FF000000000080000000000000000000000000000000000000008000400040000000000000008000000000000000080040FFFF000000F000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"

set REMOTE=false
set BUILDTYPE=Release
set outputPath=%cd%

if "%VSCMD_VER%"=="" (
	echo You must run this batch script from Visual Studio Developer Command Prompt
	if %0 == "%~0" pause
	goto :eof
)

set extra_flags=

if "%1"=="clean" goto :CLEAN

if "%1"=="clear" goto :CLEAN

if "%1"=="debug" (
	set BUILDTYPE=Debug
	set extra_flags=-DDEBUG
)

if "%1"=="remote_debug" (
	set BUILDTYPE=Debug
	set extra_flags=-DDEBUG
	set REMOTE=true
)

REM ==================== SampleLib =========================
REM set targetname=SampleLib
REM set intdir=x64\Debug
REM set outputElf=%intdir%\%targetname%.elf
REM set outputOelf=%intdir%\%targetname%.oelf
REM set outputStub=%targetname%.so
REM set outputPrx=%intdir%\%targetname%.sprx
REM set libraries=-lc -lkernel -lc++ -lSceSystemService -lSceUserService -lSceSysmodule
REM cd SampleLib\SampleLib
REM mkdir %intdir%
REM call :PRX

REM copy /y "%outputPrx%" %outputPath%\sce_module\%targetname%.sprx
REM del "%outputPrx%"
REM REM copy /y "%outputStub%" "%OO_PS4_TOOLCHAIN%\lib"
REM cd ..\..\

REM if not exist "%outputPath%\sce_module\%targetname%.sprx" (
	REM echo %targetname%.sprx FAILED TO BUILD
	REM pause
	REM goto :eof
REM )

REM ===================== EBOOT.BIN ========================
Rem Libraries to link in eboot.bin
set libraries=-lc -lkernel -lc++ -lSceSystemService -lSceUserService -lSceSysmodule -lSceFreeType


Rem Read the script arguments into local vars
set intdir=x64\Debug
set targetname=PS4-OpenOrbis-Mono

set outputElf=%intdir%\%targetname%.elf
set outputOelf=%intdir%\%targetname%.oelf

mkdir %intdir%

cd %targetname%

mkdir %intdir%
call :EBOOT

if not exist "eboot.bin" (
	echo EBOOT.BIN FAILED TO BUILD
	pause
	goto :eof
)

Rem Eboot cleanup
copy /y "eboot.bin" %outputPath%\eboot.bin
del "eboot.bin"
cd ..

REM ======================== MAIN.EXE ========================
cd main
set outputPath=
set targetname=
msbuild main.sln -t:Restore -p:Configuration=%BUILDTYPE%
msbuild main.sln -t:Rebuild -p:Configuration=%BUILDTYPE%
cd ..

if not exist "main\main\bin\x64\%BUILDTYPE%\main.exe" (
	echo main.exe FAILED TO BUILD
	pause
	exit
)

copy /y main\main\bin\x64\%BUILDTYPE%\main.exe .\
copy /y main\main\bin\x64\%BUILDTYPE%\main.pdb .\
copy /y main\main\bin\x64\%BUILDTYPE%\*.dll .\mono\4.5\
copy /y main\main\bin\x64\%BUILDTYPE%\*.pdb .\mono\4.5\

REM ====================== FINAL PKG ===========================

Rem Create param.sfo
%OO_PS4_TOOLCHAIN%\bin\windows\PkgTool.Core.exe sfo_new sce_sys/param.sfo
%OO_PS4_TOOLCHAIN%\bin\windows\PkgTool.Core.exe sfo_setentry sce_sys/param.sfo APP_TYPE --type Integer --maxsize 4 --value 1
%OO_PS4_TOOLCHAIN%\bin\windows\PkgTool.Core.exe sfo_setentry sce_sys/param.sfo APP_VER --type Utf8 --maxsize 8 --value %PKG_VERSION%
%OO_PS4_TOOLCHAIN%\bin\windows\PkgTool.Core.exe sfo_setentry sce_sys/param.sfo ATTRIBUTE --type Integer --maxsize 4 --value 0
%OO_PS4_TOOLCHAIN%\bin\windows\PkgTool.Core.exe sfo_setentry sce_sys/param.sfo CATEGORY --type Utf8 --maxsize 4 --value "gde"
%OO_PS4_TOOLCHAIN%\bin\windows\PkgTool.Core.exe sfo_setentry sce_sys/param.sfo FORMAT --type Utf8 --maxsize 4 --value "obs"
%OO_PS4_TOOLCHAIN%\bin\windows\PkgTool.Core.exe sfo_setentry sce_sys/param.sfo CONTENT_ID --type Utf8 --maxsize 48 --value %PKG_CONTENT_ID%
%OO_PS4_TOOLCHAIN%\bin\windows\PkgTool.Core.exe sfo_setentry sce_sys/param.sfo DOWNLOAD_DATA_SIZE --type Integer --maxsize 4 --value 0
%OO_PS4_TOOLCHAIN%\bin\windows\PkgTool.Core.exe sfo_setentry sce_sys/param.sfo SYSTEM_VER --type Integer --maxsize 4 --value 0
%OO_PS4_TOOLCHAIN%\bin\windows\PkgTool.Core.exe sfo_setentry sce_sys/param.sfo TITLE --type Utf8 --maxsize 128 --value %PKG_TITLE%
%OO_PS4_TOOLCHAIN%\bin\windows\PkgTool.Core.exe sfo_setentry sce_sys/param.sfo TITLE_ID --type Utf8 --maxsize 12 --value %PKG_TITLE_ID%
%OO_PS4_TOOLCHAIN%\bin\windows\PkgTool.Core.exe sfo_setentry sce_sys/param.sfo VERSION --type Utf8 --maxsize 8 --value %PKG_VERSION%


if "%REMOTE%"=="true" (
	mkdir "app"
	copy /y eboot.bin .\app
	copy /y *.exe .\app
	copy /y *.pdb .\app
	copy /y *.dll .\app
	xcopy /e /y mono .\app\mono\
	xcopy /e /y sce_module .\app\sce_module\
	xcopy /e /y sce_sys .\app\sce_sys\
	winnfsd . /
	goto :eof
)

Rem Get a list of assets for packaging
set module_files=
for %%f in (sce_module\\*) do set module_files=!module_files! sce_module/%%~nxf

set asset_audio_files=
for %%f in (assets\\audio\\*) do set asset_audio_files=!asset_audio_files! assets/audio/%%~nxf

set asset_fonts_files=
for %%f in (assets\\fonts\\*) do set asset_fonts_files=!asset_fonts_files! assets/fonts/%%~nxf

set asset_images_files=
for %%f in (assets\\images\\*) do set asset_images_files=!asset_images_files! assets/images/%%~nxf

set asset_misc_files=
for %%f in (assets\\misc\\*) do set asset_misc_files=!asset_misc_files! assets/misc/%%~nxf

set asset_videos_files=
for %%f in (assets\\videos\\*) do set asset_videos_files=!asset_videos_files! assets/videos/%%~nxf

set mono_libs=
for %%f in (mono\\4.5\\*) do set mono_libs=!mono_libs! mono/4.5/%%~nxf

Rem Create gp4
%OO_PS4_TOOLCHAIN%\bin\windows\create-gp4.exe -out pkg.gp4 --content-id=%PKG_CONTENT_ID% --files "eboot.bin main.exe main.pdb sce_sys/about/right.sprx sce_sys/param.sfo sce_sys/icon0.png sce_sys/pic0.png sce_sys/pic1.png mono/config %module_files% %asset_audio_files% %asset_fonts_files% %asset_images_files% %asset_misc_files% %asset_videos_files% %mono_libs%"

Rem Fix gp4
set quote=^"
set esc_quote=`!quote!
set open_tag=^<
set close_tag=^>

echo get-content^ pkg.gp4^ ^|^ ^%%{$_^ -replace^ "!open_tag!dir^ targ_name=!esc_quote!sce_sys!esc_quote!!close_tag!!quote!,!quote!!open_tag!dir^ targ_name=!esc_quote!mono!esc_quote!!close_tag!`n`t`t`t!open_tag!dir^ targ_name=!esc_quote!4.5!esc_quote!/!close_tag!`n`t`t!open_tag!/dir!close_tag!`n`t`t!open_tag!dir^ targ_name=!esc_quote!sce_sys!esc_quote!!close_tag!"} > tmp.ps1
powershell.exe -ExecutionPolicy Bypass -File tmp.ps1 > pkg_fixed.gp4
del /s /q tmp.ps1

Rem Create pkg
%OO_PS4_TOOLCHAIN%\bin\windows\PkgTool.Core.exe pkg_build pkg_fixed.gp4 .

goto :eof

:EBOOT

Rem Compile object files for all the source files

if exist ps4-libjbc (
	cd ps4-libjbc
	for %%f in (*.c) do (
		clang --target=x86_64-pc-freebsd12-elf -fPIC -funwind-tables -I"%OO_PS4_TOOLCHAIN%\\include" -I"%OO_PS4_TOOLCHAIN%\\include\\c++\\v1" %extra_flags% -c -o %%~nf.o %%~nf.c
	)

	cd ..
	move ps4-libjbc\*o %intdir%
)

for %%f in (*.c) do (
    clang --target=x86_64-pc-freebsd12-elf -fPIC -funwind-tables -I"%OO_PS4_TOOLCHAIN%\\include" -I"%OO_PS4_TOOLCHAIN%\\include\\c++\\v1" %extra_flags% -c -o %intdir%\%%~nf.o %%~nf.c
)

for %%f in (*.cpp) do (
    clang++ --target=x86_64-pc-freebsd12-elf -fPIC -funwind-tables -I"%OO_PS4_TOOLCHAIN%\\include" -I"%OO_PS4_TOOLCHAIN%\\include\\c++\\v1" %extra_flags% -c -o %intdir%\%%~nf.o %%~nf.cpp
)

Rem Get a list of object files for linking
set obj_files=
for %%f in (%intdir%\\*.o) do set obj_files=!obj_files! %%f

echo !obj_files!

Rem Link the input ELF
ld.lld -m elf_x86_64 -pie --script "%OO_PS4_TOOLCHAIN%\link.x" --eh-frame-hdr -o "%outputElf%" "-L%OO_PS4_TOOLCHAIN%\\lib" %libraries% --verbose "%OO_PS4_TOOLCHAIN%\lib\crt1.o" %obj_files%

Rem Create the eboot
%OO_PS4_TOOLCHAIN%\bin\windows\create-fself.exe -in "%outputElf%" --out "%outputOelf%" --eboot "eboot.bin" --paid 0x3800000000000010 --authinfo %sys_authinfo%
goto :eof


:PRX
if exist ps4-libjbc (
	cd ps4-libjbc
	for %%f in (*.c) do (
		clang --target=x86_64-pc-freebsd12-elf -fPIC -funwind-tables -I"%OO_PS4_TOOLCHAIN%\\include" -I"%OO_PS4_TOOLCHAIN%\\include\\c++\\v1" %extra_flags% -c -o %%~nf.o %%~nf.c
		clang -target x86_64-pc-linux-gnu -ffreestanding -nostdlib -fno-builtin -fPIC -c -I"%OO_PS4_TOOLCHAIN%\include" -o ..\%intdir%\%%~nf.o.stub %%~nf.c
	)

	cd ..
	move ps4-libjbc\*.o %intdir%
)
Rem Compile object files for all the source files 
for %%f in (*.c) do (
    clang --target=x86_64-pc-freebsd12-elf -fPIC -funwind-tables -I"%OO_PS4_TOOLCHAIN%\\include" %extra_flags% -c -o %intdir%\%%~nf.o %%~nf.c
)

for %%f in (*.cpp) do (
    clang++ --target=x86_64-pc-freebsd12-elf -fPIC -funwind-tables -I"%OO_PS4_TOOLCHAIN%\\include" -I"%OO_PS4_TOOLCHAIN%\\include\\c++\\v1" %extra_flags% -c -o %intdir%\%%~nf.o %%~nf.cpp
)

Rem Get a list of object files for linking
set obj_files=
for %%f in (%intdir%\\*.o) do set obj_files=!obj_files! .\%%f

Rem Link the input ELF
ld.lld -m elf_x86_64 -pie --script "%OO_PS4_TOOLCHAIN%\link.x" --eh-frame-hdr -o "%outputElf%" "-L%OO_PS4_TOOLCHAIN%\lib" %libraries% --verbose "%OO_PS4_TOOLCHAIN%\lib\crtlib.o" %obj_files%

Rem Create stub shared libraries
for %%f in (*.c) do (
    clang -target x86_64-pc-linux-gnu -ffreestanding -nostdlib -fno-builtin -fPIC -c -I"%OO_PS4_TOOLCHAIN%\include" -o %intdir%\%%~nf.o.stub %%~nf.c
)

for %%f in (*.cpp) do (
    clang++ -target x86_64-pc-linux-gnu -ffreestanding -nostdlib -fno-builtin -fPIC -c -I"%OO_PS4_TOOLCHAIN%\include" -I"%OO_PS4_TOOLCHAIN%\\include\\c++\\v1" -o %intdir%\%%~nf.o.stub %%~nf.cpp
)

set stub_obj_files=
for %%f in (%intdir%\\*.o.stub) do set stub_obj_files=!stub_obj_files! .\%%f

clang++ -target x86_64-pc-linux-gnu -shared -fuse-ld=lld -ffreestanding -nostdlib -fno-builtin "-L%OO_PS4_TOOLCHAIN%\lib" %libraries% %stub_obj_files% -o "%outputStub%"

Rem Create the prx
%OO_PS4_TOOLCHAIN%\bin\windows\create-fself.exe -in "%outputElf%" --out "%outputOelf%" --lib "%outputPrx%" --paid 0x3800000000000011
goto :eof

:CLEAN
del /s /q pkg.gp4 eboot.bin pkg_fixed.gp4 libSDL2.sprx main.exe main.pdb
del /s /q %PKG_CONTENT_ID%.pkg
rmdir /s /q PS4-OpenOrbis-Mono\x64
rmdir /s /q x64
rmdir /s /q main\main\bin
rmdir /s /q main\main\obj
rmdir /s /q main\SDL2-CS\bin
rmdir /s /q main\SDL2-CS\obj
rmdir /s /q app
del /s *.out
del /s *.so
del /s *.o
goto :eof