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
include Prt/CMakeFiles/Prt.dir/depend.make

# Include the progress variables for this target.
include Prt/CMakeFiles/Prt.dir/progress.make

# Include the compile flags for this target's objects.
include Prt/CMakeFiles/Prt.dir/flags.make

Prt/CMakeFiles/Prt.dir/Core/PrtExecution.c.o: Prt/CMakeFiles/Prt.dir/flags.make
Prt/CMakeFiles/Prt.dir/Core/PrtExecution.c.o: /Users/paulshao/P/Src/Prt/Core/PrtExecution.c
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/Users/paulshao/P/Bld/build/CMakeFiles --progress-num=$(CMAKE_PROGRESS_1) "Building C object Prt/CMakeFiles/Prt.dir/Core/PrtExecution.c.o"
	cd /Users/paulshao/P/Bld/build/Prt && /Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -o CMakeFiles/Prt.dir/Core/PrtExecution.c.o   -c /Users/paulshao/P/Src/Prt/Core/PrtExecution.c

Prt/CMakeFiles/Prt.dir/Core/PrtExecution.c.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing C source to CMakeFiles/Prt.dir/Core/PrtExecution.c.i"
	cd /Users/paulshao/P/Bld/build/Prt && /Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -E /Users/paulshao/P/Src/Prt/Core/PrtExecution.c > CMakeFiles/Prt.dir/Core/PrtExecution.c.i

Prt/CMakeFiles/Prt.dir/Core/PrtExecution.c.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling C source to assembly CMakeFiles/Prt.dir/Core/PrtExecution.c.s"
	cd /Users/paulshao/P/Bld/build/Prt && /Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -S /Users/paulshao/P/Src/Prt/Core/PrtExecution.c -o CMakeFiles/Prt.dir/Core/PrtExecution.c.s

Prt/CMakeFiles/Prt.dir/Core/PrtTypes.c.o: Prt/CMakeFiles/Prt.dir/flags.make
Prt/CMakeFiles/Prt.dir/Core/PrtTypes.c.o: /Users/paulshao/P/Src/Prt/Core/PrtTypes.c
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/Users/paulshao/P/Bld/build/CMakeFiles --progress-num=$(CMAKE_PROGRESS_2) "Building C object Prt/CMakeFiles/Prt.dir/Core/PrtTypes.c.o"
	cd /Users/paulshao/P/Bld/build/Prt && /Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -o CMakeFiles/Prt.dir/Core/PrtTypes.c.o   -c /Users/paulshao/P/Src/Prt/Core/PrtTypes.c

Prt/CMakeFiles/Prt.dir/Core/PrtTypes.c.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing C source to CMakeFiles/Prt.dir/Core/PrtTypes.c.i"
	cd /Users/paulshao/P/Bld/build/Prt && /Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -E /Users/paulshao/P/Src/Prt/Core/PrtTypes.c > CMakeFiles/Prt.dir/Core/PrtTypes.c.i

Prt/CMakeFiles/Prt.dir/Core/PrtTypes.c.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling C source to assembly CMakeFiles/Prt.dir/Core/PrtTypes.c.s"
	cd /Users/paulshao/P/Bld/build/Prt && /Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -S /Users/paulshao/P/Src/Prt/Core/PrtTypes.c -o CMakeFiles/Prt.dir/Core/PrtTypes.c.s

Prt/CMakeFiles/Prt.dir/Core/PrtValues.c.o: Prt/CMakeFiles/Prt.dir/flags.make
Prt/CMakeFiles/Prt.dir/Core/PrtValues.c.o: /Users/paulshao/P/Src/Prt/Core/PrtValues.c
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/Users/paulshao/P/Bld/build/CMakeFiles --progress-num=$(CMAKE_PROGRESS_3) "Building C object Prt/CMakeFiles/Prt.dir/Core/PrtValues.c.o"
	cd /Users/paulshao/P/Bld/build/Prt && /Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -o CMakeFiles/Prt.dir/Core/PrtValues.c.o   -c /Users/paulshao/P/Src/Prt/Core/PrtValues.c

Prt/CMakeFiles/Prt.dir/Core/PrtValues.c.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing C source to CMakeFiles/Prt.dir/Core/PrtValues.c.i"
	cd /Users/paulshao/P/Bld/build/Prt && /Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -E /Users/paulshao/P/Src/Prt/Core/PrtValues.c > CMakeFiles/Prt.dir/Core/PrtValues.c.i

Prt/CMakeFiles/Prt.dir/Core/PrtValues.c.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling C source to assembly CMakeFiles/Prt.dir/Core/PrtValues.c.s"
	cd /Users/paulshao/P/Bld/build/Prt && /Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -S /Users/paulshao/P/Src/Prt/Core/PrtValues.c -o CMakeFiles/Prt.dir/Core/PrtValues.c.s

Prt/CMakeFiles/Prt.dir/LinuxUser/PrtLinuxUserConfig.c.o: Prt/CMakeFiles/Prt.dir/flags.make
Prt/CMakeFiles/Prt.dir/LinuxUser/PrtLinuxUserConfig.c.o: /Users/paulshao/P/Src/Prt/LinuxUser/PrtLinuxUserConfig.c
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/Users/paulshao/P/Bld/build/CMakeFiles --progress-num=$(CMAKE_PROGRESS_4) "Building C object Prt/CMakeFiles/Prt.dir/LinuxUser/PrtLinuxUserConfig.c.o"
	cd /Users/paulshao/P/Bld/build/Prt && /Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -o CMakeFiles/Prt.dir/LinuxUser/PrtLinuxUserConfig.c.o   -c /Users/paulshao/P/Src/Prt/LinuxUser/PrtLinuxUserConfig.c

Prt/CMakeFiles/Prt.dir/LinuxUser/PrtLinuxUserConfig.c.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing C source to CMakeFiles/Prt.dir/LinuxUser/PrtLinuxUserConfig.c.i"
	cd /Users/paulshao/P/Bld/build/Prt && /Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -E /Users/paulshao/P/Src/Prt/LinuxUser/PrtLinuxUserConfig.c > CMakeFiles/Prt.dir/LinuxUser/PrtLinuxUserConfig.c.i

Prt/CMakeFiles/Prt.dir/LinuxUser/PrtLinuxUserConfig.c.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling C source to assembly CMakeFiles/Prt.dir/LinuxUser/PrtLinuxUserConfig.c.s"
	cd /Users/paulshao/P/Bld/build/Prt && /Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -S /Users/paulshao/P/Src/Prt/LinuxUser/PrtLinuxUserConfig.c -o CMakeFiles/Prt.dir/LinuxUser/PrtLinuxUserConfig.c.s

Prt: Prt/CMakeFiles/Prt.dir/Core/PrtExecution.c.o
Prt: Prt/CMakeFiles/Prt.dir/Core/PrtTypes.c.o
Prt: Prt/CMakeFiles/Prt.dir/Core/PrtValues.c.o
Prt: Prt/CMakeFiles/Prt.dir/LinuxUser/PrtLinuxUserConfig.c.o
Prt: Prt/CMakeFiles/Prt.dir/build.make

.PHONY : Prt

# Rule to build all files generated by this target.
Prt/CMakeFiles/Prt.dir/build: Prt

.PHONY : Prt/CMakeFiles/Prt.dir/build

Prt/CMakeFiles/Prt.dir/clean:
	cd /Users/paulshao/P/Bld/build/Prt && $(CMAKE_COMMAND) -P CMakeFiles/Prt.dir/cmake_clean.cmake
.PHONY : Prt/CMakeFiles/Prt.dir/clean

Prt/CMakeFiles/Prt.dir/depend:
	cd /Users/paulshao/P/Bld/build && $(CMAKE_COMMAND) -E cmake_depends "Unix Makefiles" /Users/paulshao/P/Src /Users/paulshao/P/Src/Prt /Users/paulshao/P/Bld/build /Users/paulshao/P/Bld/build/Prt /Users/paulshao/P/Bld/build/Prt/CMakeFiles/Prt.dir/DependInfo.cmake --color=$(COLOR)
.PHONY : Prt/CMakeFiles/Prt.dir/depend

