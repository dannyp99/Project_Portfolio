#!/bin/bash
if [[ $1 = "-h" ]] || [[ $1 = "--help" ]];then
	echo "-h,--help			View all options and arguments for the script."
	echo "--clean-up		Run brew cleanup after uninstalling."
fi

if [[ -d "$HOME/.config/omf" ]]; then
	rm -rf ~/.config/omf
	rm -rf ~/.local/share/omf
	echo "omf removed"
else
	echo "omf not installed"
fi

if [[ -f "/usr/bin/fish" ]]; then
	rm -rf ~/.config/fish
	brew remove fish
	echo "fish removed"
else
    echo "fish not installed"
fi

if [[ $1 = "--clean-up" ]];then
	brew cleanup
fi
exit
