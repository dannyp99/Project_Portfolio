These are some functions I made for shells I use as well as some common shells

mkcd
	- Make a dir and cd into it
up 
	- the command followed by a number will do cd followed by ../ * the number passed
	- up 3 == cd ../../../
	- doesn't break cd - // command to return to prev pwd 

almighypush
	- git add -A, commit and push to HEAD all in one

kiryu
	- This started as a joke, but with quarantine I decided to do it.
	- requires mpv (brew install mpv/apt install mpv/choco install mpv) (mac/linux/windows)
	- This will download a a youtube video of kiryu from Yakuza 0 slamming a desk in frustration.
	- The function is called after every command and if the command returns error code 1 or 127 (command not found)
	  the video will play for 2 seconds and mpv will not output to stdout.
	- I added the mp4 if you put it in your Downloads folder it should run with mpv and not download the video.
