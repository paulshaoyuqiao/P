# CMAKE generated file: DO NOT EDIT!
# Generated by "Unix Makefiles" Generator, CMake Version 3.13

# Delete rule output on recipe failure.
.DELETE_ON_ERROR:


#=============================================================================
# Special targets provided by cmake.

# Disable implicit rules so canonical targets will work.
.SUFFIXES:


# Remove some rules from gmake that .SUFFIXES does not remove.
SUFFIXES =

.SUFFIXES: .hpux_make_needs_suffix_list


# Suppress display of executed commands.
$(VERBOSE).SILENT:


# A target that is always out of date.
cmake_force:

.PHONY : cmake_force

#=============================================================================
# Set environment variables for the build.

# The shell in which to execute make rules.
SHELL = /bin/sh

# The CMake executable.
CMAKE_COMMAND = /usr/local/Cellar/cmake/3.13.2/bin/cmake

# The command to remove a file.
RM = /usr/local/Cellar/cmake/3.13.2/bin/cmake -E remove -f

# Escaping for special characters.
EQUALS = =

# The top-level source directory on which CMake was run.
CMAKE_SOURCE_DIR = /Users/paulshao/P/Src

# The top-level build directory on which CMake was run.
CMAKE_BINARY_DIR = /Users/paulshao/P/Bld/build

# Include any dependencies generated for this target.
include Prt/CMakeFiles/Prt_shared.dir/depend.make

# Include the progress variables for this target.
include Prt/CMakeFiles/Prt_shared.dir/progress.make

# Include the compile flags for this target's objects.
include Prt/CMakeFiles/Prt_shared.dir/flags.make

# Object files for target Prt_shared
Prt_shared_OBJECTS =

# External object files for target Prt_shared
Prt_shared_EXTERNAL_OBJECTS = \
"/Users/paulshao/P/Bld/build/Prt/CMakeFiles/Prt.dir/Core/PrtExecution.c.o" \
"/Users/paulshao/P/Bld/build/Prt/CMakeFiles/Prt.dir/Core/PrtTypes.c.o" \
"/Users/paulshao/P/Bld/build/Prt/CMakeFiles/Prt.dir/Core/PrtValues.c.o" \
"/Users/paulshao/P/Bld/build/Prt/CMakeFiles/Prt.dir/LinuxUser/PrtLinuxUserConfig.c.o"

/Users/paulshao/P/Bld/Drops/Prt/lib/libPrt_shared.dylib: Prt/CMakeFiles/Prt.dir/Core/PrtExecution.c.o
/Users/paulshao/P/Bld/Drops/Prt/lib/libPrt_shared.dylib: Prt/CMakeFiles/Prt.dir/Core/PrtTypes.c.o
/Users/paulshao/P/Bld/Drops/Prt/lib/libPrt_shared.dylib: Prt/CMakeFiles/Prt.dir/Core/PrtValues.c.o
/Users/paulshao/P/Bld/Drops/Prt/lib/libPrt_shared.dylib: Prt/CMakeFiles/Prt.dir/LinuxUser/PrtLinuxUserConfig.c.o
/Users/paulshao/P/Bld/Drops/Prt/lib/libPrt_shared.dylib: Prt/CMakeFiles/Prt_shared.dir/build.make
/Users/paulshao/P/Bld/Drops/Prt/lib/libPrt_shared.dylib: Prt/CMakeFiles/Prt_shared.dir/link.txt
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --bold --progress-dir=/Users/paulshao/P/Bld/build/CMakeFiles --progress-num=$(CMAKE_PROGRESS_1) "Linking C shared library /Users/paulshao/P/Bld/Drops/Prt/lib/libPrt_shared.dylib"
	cd /Users/paulshao/P/Bld/build/Prt && $(CMAKE_COMMAND) -E cmake_link_script CMakeFiles/Prt_shared.dir/link.txt --verbose=$(VERBOSE)
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --blue --bold "Moving header files to Bld/include/"
	cd /Users/paulshao/P/Bld/build/Prt && /usr/local/Cellar/cmake/3.13.2/bin/cmake -E make_directory /Users/paulshao/P/Src/../Bld/Drops/Prt//include/
	cd /Users/paulshao/P/Bld/build/Prt && /usr/local/Cellar/cmake/3.13.2/bin/cmake -E copy_if_different /Users/paulshao/P/Src/Prt/API//Prt.h /Users/paulshao/P/Src/../Bld/Drops/Prt//include/
	cd /Users/paulshao/P/Bld/build/Prt && /usr/local/Cellar/cmake/3.13.2/bin/cmake -E copy_if_different /Users/paulshao/P/Src/Prt/API//PrtConfig.h /Users/paulshao/P/Src/../Bld/Drops/Prt//include/
	cd /Users/paulshao/P/Bld/build/Prt && /usr/local/Cellar/cmake/3.13.2/bin/cmake -E copy_if_different /Users/paulshao/P/Src/Prt/API//PrtProgram.h /Users/paulshao/P/Src/../Bld/Drops/Prt//include/
	cd /Users/paulshao/P/Bld/build/Prt && /usr/local/Cellar/cmake/3.13.2/bin/cmake -E copy_if_different /Users/paulshao/P/Src/Prt/API//PrtTypes.h /Users/paulshao/P/Src/../Bld/Drops/Prt//include/
	cd /Users/paulshao/P/Bld/build/Prt && /usr/local/Cellar/cmake/3.13.2/bin/cmake -E copy_if_different /Users/paulshao/P/Src/Prt/API//PrtValues.h /Users/paulshao/P/Src/../Bld/Drops/Prt//include/
	cd /Users/paulshao/P/Bld/build/Prt && /usr/local/Cellar/cmake/3.13.2/bin/cmake -E copy_if_different /Users/paulshao/P/Src/Prt/../../Ext/libhandler/inc/libhandler-internal.h /Users/paulshao/P/Src/../Bld/Drops/Prt//include/
	cd /Users/paulshao/P/Bld/build/Prt && /usr/local/Cellar/cmake/3.13.2/bin/cmake -E copy_if_different /Users/paulshao/P/Src/Prt/../../Ext/libhandler/inc/libhandler.h /Users/paulshao/P/Src/../Bld/Drops/Prt//include/
	cd /Users/paulshao/P/Bld/build/Prt && /usr/local/Cellar/cmake/3.13.2/bin/cmake -E copy_if_different /Users/paulshao/P/Src/Prt/Core//PrtExecution.h /Users/paulshao/P/Src/../Bld/Drops/Prt//include/
	cd /Users/paulshao/P/Bld/build/Prt && /usr/local/Cellar/cmake/3.13.2/bin/cmake -E copy_if_different /Users/paulshao/P/Src/Prt/LinuxUser//PrtLinuxUserConfig.h /Users/paulshao/P/Src/../Bld/Drops/Prt//include/
	cd /Users/paulshao/P/Bld/build/Prt && /usr/local/Cellar/cmake/3.13.2/bin/cmake -E copy_if_different /Users/paulshao/P/Src/Prt/LinuxUser//ext_compat.h /Users/paulshao/P/Src/../Bld/Drops/Prt//include/
	cd /Users/paulshao/P/Bld/build/Prt && /usr/local/Cellar/cmake/3.13.2/bin/cmake -E copy_if_different /Users/paulshao/P/Src/Prt/LinuxUser//sal.h /Users/paulshao/P/Src/../Bld/Drops/Prt//include/

# Rule to build all files generated by this target.
Prt/CMakeFiles/Prt_shared.dir/build: /Users/paulshao/P/Bld/Drops/Prt/lib/libPrt_shared.dylib

.PHONY : Prt/CMakeFiles/Prt_shared.dir/build

Prt/CMakeFiles/Prt_shared.dir/clean:
	cd /Users/paulshao/P/Bld/build/Prt && $(CMAKE_COMMAND) -P CMakeFiles/Prt_shared.dir/cmake_clean.cmake
.PHONY : Prt/CMakeFiles/Prt_shared.dir/clean

Prt/CMakeFiles/Prt_shared.dir/depend:
	cd /Users/paulshao/P/Bld/build && $(CMAKE_COMMAND) -E cmake_depends "Unix Makefiles" /Users/paulshao/P/Src /Users/paulshao/P/Src/Prt /Users/paulshao/P/Bld/build /Users/paulshao/P/Bld/build/Prt /Users/paulshao/P/Bld/build/Prt/CMakeFiles/Prt_shared.dir/DependInfo.cmake --color=$(COLOR)
.PHONY : Prt/CMakeFiles/Prt_shared.dir/depend

