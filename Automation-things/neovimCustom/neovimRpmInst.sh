#!/bin/bash
sudo yum update

if [[ ! -f "/usr/bin/wget" ]]; then
	sudo yum install wget
fi
if [[ ! -f "/usr/bin/nvim" ]]; then
	sudo yum install neovim
	echo "neovim installed"
else
	echo "neovim already installed"
fi

if [[ ! -d "$HOME/.config/nvim/colors" ]]; then
	mkdir -p ~/.config/nvim/colors
fi

if [[ ! -f "$HOME/.config/nvim/colors/molokai.vim" ]]; then
	wget https://raw.githubusercontent.com/tomasr/molokai/master/colors/molokai.vim
	mv molokai.vim ~/.config/nvim/colors
	echo "configuring neovim theme..."
else
	echo "color theme already set"
fi

if [[ ! -d "$HOME/.config/nvim/pack/vendor/start/nerdtree" ]]; then
	echo "configuring NERDTree..."
	git clone https://github.com/preservim/nerdtree.git ~/.config/nvim/pack/vendor/start/nerdtree
	cd ~/.config/nvim
	touch init.vim
	echo "syntax on" >> init.vim
	echo "colorscheme molokai" >> init.vim 
	echo "set ts=4 sw=4" >> init.vim
	echo "autocmd StdinReadPre * let s:std_in=1" >> init.vim
	echo "autocmd VimEnter * if argc() == 0 && !exists(\"s:std_in\") | NERDTree | endif" >> init.vim 
	echo "autocmd StdinReadPre * let s:std_in=1" >> init.vim 
	echo "autocmd VimEnter * if argc() == 1 && isdirectory(argv()[0]) && !exists(\"s:std_in\") | exe 'NERDTree' argv()[0] | wincmd p | ene | exe 'cd '.argv()[0] | endif" >> init.vim 
	echo "autocmd bufenter * if (winnr(\"$\") == 1 && exists(\"b:NERDTree\") && b:NERDTree.isTabTree()) | q | endif" >> init.vim 
else
	echo "NERDTree already installed"
fi
