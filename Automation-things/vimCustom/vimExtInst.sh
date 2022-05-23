#!/bin/bash

if [[ ! -f $HOME/.vim/autoload/plug.vim ]]; then
    curl -fLo $HOME/.vim/autoload/plug.vim --create-dirs \
        https://raw.githubusercontent.com/junegunn/vim-plug/master/plug.vim
fi

if [[ ! -f $HOME/.vim/colors/PaperColor.vim ]];then 
		curl -fLo $HOME/.vim/colors/PaperColor.vim --create-dirs \
			https://raw.githubusercontent.com/NLKNguyen/papercolor-theme/master/colors/PaperColor.vim 
fi

if [[ ! -f $HOME/.vimrc ]];then
    echo "syntax on" > $HOME/.vimrc
    echo "set nu" >> $HOME/.vimrc
    echo "set background=dark" >> $HOME/.vimrc
    echo "colorscheme PaperColor" >> $HOME/.vimrc
    echo "set ts=4 sw=4" >> $HOME/.vimrc
    echo "" >> $HOME/.vimrc
    echo "call plug#begin()" >> $HOME/.vimrc
    echo "" >> $HOME/.vimrc
    echo "Plug 'https://github.com/scrooloose/nerdtree', { 'on': 'NERDTreeToggle' }" >> $HOME/.vimrc
    echo "Plug 'https://github.com/vim-airline/vim-airline'" >> $HOME/.vimrc
    echo "" >> $HOME/.vimrc
    echo "call plug#end()" >> $HOME/.vimrc
    echo "autocmd StdinReadPre * let s:std_in=1" >> $HOME/.vimrc
    echo "autocmd VimEnter * if argc() == 1 && isdirectory(argv()[0]) && !exists('s:std_in') | execute 'NERDTree' argv()[0] | wincmd p | enew | execute 'cd '.argv()[0] | endif" >> $HOME/.vimrc
    echo "autocmd BufEnter * if winnr('$') == 1 && exists('b:NERDTree') && b:NERDTree.isTabTree() | quit | endif" >> $HOME/.vimrc
    echo "autocmd BufEnter * if bufname('#') =~ 'NERD_tree_\d\+' && bufname('%') !~ 'NERD_tree_\d\+' && winnr('$') > 1 | let buf=bufnr() | buffer# | execute \"normal! \<C-W>w\" | execute 'buffer'.buf | endif" >> $HOME/.vimrc
    vim +'PlugInstall --sync' +qa
fi
