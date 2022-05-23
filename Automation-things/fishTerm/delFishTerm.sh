#!/bin/bash
if [[ -d "$HOME/.config/omf" ]]; then
	rm -rf ~/.config/omf
	rm -rf ~/.local/share/omf
	echo "omf removed"
else
	echo "omf not installed"
fi

if [[ -f "/usr/bin/fish" ]]; then
	rm -rf ~/.config/fish
	sudo apt -y purge fish
	sudo apt -y autoremove
	echo "fish removed"
else
    echo "fish not installed"
fi
exit
