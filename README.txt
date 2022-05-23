Automation_Things
	- Shell functions I made for convience and fun
	- bash, zsh and fish
	-Also install fish and neovim which I use for my VM. I use these to automate VM installs.

Programming_Languages

	- For my favorite class I have taken we had to projects that we had to do.
	- as7 is an assembler that will run 'assembly' instructions. the following files with a bizarre extension are text files containing such assembly
	- as8 is a compiler/interpretor for a language used in the class. The program will execute code in interpretive mode and compile to 'assembly' which
	  can be run by the assembler as7. The compiler supports up to if else while the interpreter accepts lambda expressions

	compile the fsharp as7:
		fsharpc asembler.fs -r am7b19.dll 
		mono asembler.exe  < compif.7b 

	as8:
		fsharpc compilier.fs -r compilatorbase.dll
		mono compilier.exe compile - will compile code entered
RemindMeNotes
	- This is my Final Project for my Mobile Development Course. I chose the concept myself and thought it was useful for my personal needs.
	  I find that sometimes while taking notes I want to make a reminder for myself about a quiz or test that a professor might quickly mention.
	  Maybe I remember that I need to call someone right after class and I want to just quickly make a reminder without having to leave the app entirely.

VRClassroom_backend
	- Senior Design Project API used to pull data from the Database (MariaDB) and send it to the VR application.
	- Uses KOA routing and middleware to authenticate users with JWT tokens.
	- Emulated the Repository pattern using router files to call controller functions and models to make sql calls.

parser-generator
	- Most recent project with ability to create an Abstract Syntax tree for any language you have a grammar for.
	- Optional lexical analyzer for easy implementation for your language.
	- Includes a compiler for mongoose, a language we were required to compile for the class.
	- Compiler generates valid LLVM file that can then be compiled to a binary with clang.
	- WARNING since this was a semester project, we developed and the project in Linux for Linux. We HAVE NOT tested this outside of Linux and can't guarantee it will work on other OSs.
