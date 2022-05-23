#!/bin/bash
if [[ "$1" = "-h" ]] || [[ "$1" = "--help" ]];then
	echo "-h,help 				View all arguments for the script"
	echo "--remove-all			remove all programs added through the install this includes wget"
	echo "						default running will remove oh-my-zsh as well as zsh"
	exit
fi

if [[ "$1" = "--remove-all" ]];then
	sudo apt -y purge wget
fi

if [[ -f "/usr/bin/font-manager" ]];then
	sudo apt -y purge font-manager
fi

if [[ "$SHELL" = "/bin/zsh" ]];then
	echo "Can't unsinstall zsh, zsh is still the default shell"
	echo "Please use chsh command to change your shell"
else
	sudo apt -y purge zsh
	rm $HOME/.zshrc
	rm $HOME/.zsh_history
fi

if [[ -d "$HOME/.oh-my-zsh" ]];then
	sudo rm -rf $HOME/.oh-my-zsh
else 
	echo "oh-my-zsh is already deleted"
fi

sudo apt -y autoremove
