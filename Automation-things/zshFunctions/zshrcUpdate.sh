#!/bin/bash
if [[ $1 = "-h" ]] || [[ $1 = "--help" ]];then
	echo "-h,--help			See all arguments for this script"
	echo "--update-zshrc	Modify current zshrc to be zshrcFunc.txt backup created (prev_zshrc)"
fi

if [[ $1 = "--update-zshrc" ]];then
	cp $HOME/.zshrc prev_zshrc
	cat zshrcFunc.txt >> ~/.zshr
fi

if [[ -f "./kiryu.mp4" ]] && [[ ! -f "$HOME/Downloads/kiryu.mp4" ]];then
		cp ./kiryu.mp4 $HOME/Downloads
fi

if [[ -f "./push.mp4" ]] && [[ ! -f "$HOME/Downloads/push.mp4" ]];then
	cp ./push.mp4 $HOME/Downloads
fi
