#!/bin/bash
if [[ -d "$HOME/.config/nvim" ]]; then
	rm -rf ~/.config/nvim
	echo "neovim config removed"
else
	echo "neovim config doesn't exist"
fi

if [[  -f "/usr/local/bin/nvim" ]]; then
	brew remove neovim
	echo "neovim removed"
else
	echo "neovim not installed"
fi
