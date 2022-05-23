#!/bin/bash
if [[  -d "$HOME/.config/nvim" ]]; then
	rm -rf ~/.config/nvim
	echo "neovim config removed"
else
	echo "neovim config doesn't exist"
fi

if [[ -f "/usr/bin/nvim" ]]; then
	sudo apt purge neovim
	sudo apt autoremove
	echo "neovim removed"
else
	echo "neovim not installed"
fi
