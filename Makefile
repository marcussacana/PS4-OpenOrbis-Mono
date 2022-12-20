# Package metadata.
TITLE       := OpenOrbis Mono Sample
VERSION     := 1.00
TITLE_ID    := MONO00001
CONTENT_ID  := IV0000-MONO00001_00-MONOSAMPLE000000

# Libraries linked into the ELF.
LIBS        := -lc -lkernel -lc++ -lSceSystemService -lSceUserService -lSceSysmodule -lSceMsgDialog -lSceFreeType

# Additional compile flags.
#EXTRAFLAGS  := 
BUILDTYPE   := Release

# Asset and module directories.
ASSETS 		:= $(wildcard assets/**/*)
LIBMODULES  := $(wildcard sce_module/*) sce_module/libSDL2.sprx


# You likely won't need to touch anything below this point.

# Root vars
TOOLCHAIN   := $(OO_PS4_TOOLCHAIN)
PROJDIR     := $(shell basename $(CURDIR))
COMMONDIR   := $(TOOLCHAIN)/samples/_common
INTDIR      := $(PROJDIR)/x64/Debug

# Define objects to build
JBFILES      := $(wildcard $(PROJDIR)/ps4-libjbc/*.c)
CFILES      := $(wildcard $(PROJDIR)/*.c)
CPPFILES    := $(wildcard $(PROJDIR)/*.cpp)
OBJS        := $(patsubst $(PROJDIR)/%.c, $(INTDIR)/%.o, $(CFILES)) $(patsubst $(PROJDIR)/%.cpp, $(INTDIR)/%.o, $(CPPFILES)) $(patsubst $(PROJDIR)/ps4-libjbc/%.c, $(INTDIR)/%.o, $(JBFILES))

# Define final C/C++ flags
CFLAGS      := --target=x86_64-pc-freebsd12-elf -fPIC -funwind-tables -c $(EXTRAFLAGS) -isysroot $(TOOLCHAIN) -isystem $(TOOLCHAIN)/include
CXXFLAGS    := $(CFLAGS) -isystem $(TOOLCHAIN)/include/c++/v1
LDFLAGS     := --error-limit=0 -m elf_x86_64 -pie --script $(TOOLCHAIN)/link.x --eh-frame-hdr -L$(TOOLCHAIN)/lib $(LIBS) $(TOOLCHAIN)/lib/crt1.o

# Create the intermediate directory incase it doesn't already exist.
_unused     := $(shell mkdir -p $(INTDIR))

# Check for linux vs macOS and account for clang/ld path
UNAME_S     := $(shell uname -s)

ifeq ($(UNAME_S),Linux)
		CC      := clang
		CCX     := clang++
		LD      := ld.lld
		CDIR    := linux
endif
ifeq ($(UNAME_S),Darwin)
		CC      := /usr/local/opt/llvm/bin/clang
		CCX     := /usr/local/opt/llvm/bin/clang++
		LD      := /usr/local/opt/llvm/bin/ld.lld
		CDIR    := macos
endif

#DotNet Bundle Fix

all: $(CONTENT_ID).pkg

$(CONTENT_ID).pkg: export DOTNET_BUNDLE_EXTRACT_BASE_DIR=$(TOOLCHAIN)/bin/$(CDIR)/tmp
$(CONTENT_ID).pkg: main.exe 
$(CONTENT_ID).pkg: pkg.gp4
	$(TOOLCHAIN)/bin/$(CDIR)/PkgTool.Core pkg_build $< .

pkg.gp4: eboot.bin main.exe main.pdb sce_sys/about/right.sprx sce_sys/param.sfo sce_sys/icon0.png sce_sys/pic0.png sce_sys/pic1.png $(LIBMODULES) $(ASSETS)
	$(TOOLCHAIN)/bin/$(CDIR)/create-gp4 -out $@ --content-id=$(CONTENT_ID) --files "$^ $(wildcard mono/**/*)"
	sed -i "s/<dir targ_name=\"sce_sys\">/<dir targ_name=\"mono\">\n\t\t\t<dir targ_name=\"4.5\" \/>\n\t\t<\/dir>\n\t\t<dir targ_name=\"sce_sys\">/" pkg.gp4
	

main.exe: sce_module/libSDL2.sprx
	msbuild main/main.sln -t:Restore -p:Configuration=$(BUILDTYPE)
	msbuild main/main.sln -t:Rebuild -p:Configuration=$(BUILDTYPE)
	mv -f main/main/bin/x64/$(BUILDTYPE)/main.exe ./main.exe
	mv -f main/main/bin/x64/$(BUILDTYPE)/main.pdb ./main.pdb
	-cp -f main/main/bin/x64/$(BUILDTYPE)/*.dll ./mono/4.5/
	-cp -f main/main/bin/x64/$(BUILDTYPE)/*.pdb ./mono/4.5/

sce_module/libSDL2.sprx:
	make -C libSDL2
	cp -f libSDL2/libSDL2.sprx sce_module/libSDL2.sprx

sce_sys/param.sfo: Makefile
	$(TOOLCHAIN)/bin/$(CDIR)/PkgTool.Core sfo_new $@
	$(TOOLCHAIN)/bin/$(CDIR)/PkgTool.Core sfo_setentry $@ APP_TYPE --type Integer --maxsize 4 --value 1 
	$(TOOLCHAIN)/bin/$(CDIR)/PkgTool.Core sfo_setentry $@ APP_VER --type Utf8 --maxsize 8 --value '$(VERSION)'
	$(TOOLCHAIN)/bin/$(CDIR)/PkgTool.Core sfo_setentry $@ ATTRIBUTE --type Integer --maxsize 4 --value 0  
	$(TOOLCHAIN)/bin/$(CDIR)/PkgTool.Core sfo_setentry $@ CATEGORY --type Utf8 --maxsize 4 --value 'gde'  
	$(TOOLCHAIN)/bin/$(CDIR)/PkgTool.Core sfo_setentry $@ CONTENT_ID --type Utf8 --maxsize 48 --value '$(CONTENT_ID)'
	$(TOOLCHAIN)/bin/$(CDIR)/PkgTool.Core sfo_setentry $@ DOWNLOAD_DATA_SIZE --type Integer --maxsize 4 --value 0 
	$(TOOLCHAIN)/bin/$(CDIR)/PkgTool.Core sfo_setentry $@ SYSTEM_VER --type Integer --maxsize 4 --value 0  
	$(TOOLCHAIN)/bin/$(CDIR)/PkgTool.Core sfo_setentry $@ TITLE --type Utf8 --maxsize 128 --value '$(TITLE)'
	$(TOOLCHAIN)/bin/$(CDIR)/PkgTool.Core sfo_setentry $@ TITLE_ID --type Utf8 --maxsize 12 --value '$(TITLE_ID)'
	$(TOOLCHAIN)/bin/$(CDIR)/PkgTool.Core sfo_setentry $@ VERSION --type Utf8 --maxsize 8 --value '$(VERSION)'

eboot.bin: $(INTDIR) $(OBJS)
	$(LD) $(INTDIR)/*.o -o $(INTDIR)/$(PROJDIR).elf $(LDFLAGS)
	$(TOOLCHAIN)/bin/$(CDIR)/create-fself -in=$(INTDIR)/$(PROJDIR).elf -out=$(INTDIR)/$(PROJDIR).oelf --eboot "eboot.bin" --paid 0x3800000000000011
	
$(INTDIR)/%.o: $(PROJDIR)/ps4-libjbc/%.c
	$(CC) $(CFLAGS) -o $@ $<
	
$(INTDIR)/%.o: $(PROJDIR)/%.c
	$(CC) $(DEBUG) $(CFLAGS) -o $@ $<

$(INTDIR)/%.o: $(PROJDIR)/%.cpp
	$(CCX) $(DEBUG) $(CXXFLAGS) -o $@ $<
	

debug: DEBUG      := --define-macro DEBUG
debug: BUILDTYPE  := Debug
debug: all

clear: clean

clean:
	rm -f -r $(CONTENT_ID).pkg pkg.gp4 sce_module/libSDL2.sprx pkg/sce_sys/param.sfo eboot.bin main.exe main.pdb mono/4.5/*.pdb $(PROJDIR)/x64
	make -C libSDL2 clean
