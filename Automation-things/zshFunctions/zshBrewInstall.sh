#!/bin/bash
if [[ $1 = "-h" ]] || [[ $1 = "--help" ]];then
	echo "-h,--help 		View all arguments for this bash script"
	echo "--set-zsh			Set zsh to defualt terminal"
	exit
fi

if [[ -z $(which brew) ]] || [[ ! -f "/usr/local/bin/brew"  ]];then
	echo "Please install brew, a package manager for Mac"
	echo "You can install it with:"
	echo "/bin/bash -c \"\$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install.sh)\""
	exit
fi

if [[ -f "/usr/bin/zsh" ]] || [[ -f "/bin/zsh" ]];then 
	echo "zsh is already installed"
else 
	brew install zsh
fi

if [[  -d "$HOME/.oh-my-zsh" ]];then
	echo "oh-my-zsh is already installed"
else 
	sh -c "\$(curl -fsSL https://raw.githubusercontent.com/robbyrussell/oh-my-zsh/master/tools/install.sh)"
fi

if [[ $1 = "--set-zsh" ]];then
	chsh -s "$(which zsh)"
fi

if [[ -f "$HOME/.oh-my-zsh/custom/themes/powerlevel10k" ]];then
	echo "powerlevel10k is already installed"
else
	git clone https://github.com/romkatv/powerlevel10k.git $HOME/.oh-my-zsh/custom/themes/powerlevel10k
fi

if [[ -f "/usr/share/fonts/Fira\ Mono\ Regular\ Nerd\ Font\ Complete.otf" ]] || [[ -f "/usr/local/share/fonts/Fira\ Mono\ Regular\ Nerd\ Font\ Complete.otf" ]] || [[ -f "$HOME/.fonts/Fira\ Mono\ Regular\ Nerd\ Font\ Complete.otf" ]];then
	echo "Fonts installed"
else	
	if [[ ! -f "/bin/wget" ]] || [[ ! -f "/usr/local/bin/wget" ]];then
		brew install wget
	fi
	wget -O $HOME/Downloads/Fira\ Mono\ Regular\ Nerd\ Font\ Complete.otf https://github.com/ryanoasis/nerd-fonts/blob/master/patched-fonts/FiraMono/Regular/complete/Fira%20Mono%20Regular%20Nerd%20Font%20Complete.otf?raw=true
	open $HOME/Downloads/Fira\ Mono\ Regular\ Nerd\ Font\ Complete.otf
fi

if [[ -d "$HOME/.oh-my-zsh/custom/plugins/zsh-syntax-highlighting" ]];then
	echo "plugin installed"
else 
	git clone https://github.com/zsh-users/zsh-syntax-highlighting.git $HOME/.oh-my-zsh/custom/plugins/zsh-syntax-highlighting
	git clone https://github.com/zsh-users/zsh-autosuggestions.git $HOME/.oh-my-zsh/custom/plugins/zsh-autosuggestions
	cp $HOME/.zshrc zshrc_backup
	sed -i '' 's/plugins=.*/plugins=(git zsh-autosuggestions zsh-syntax-highlighting)/' ~/.zshrc
	sed 's/ZSH_THEME.*/ZSH_THEME="powerlevel10k\/powerlevel10k"|POWERLEVEL9K_MODE="nerdfont-complete"/g' zshrc_backup | tr '|' '\n' > $HOME/.zshrc
fi
echo "All Done Please refresh your your terminal or open zsh and type 'source ~/.zshrc'"
echo "If the font is not loaded config won't work. It should be in you ~/Downloads folder open it and select install!"
exit
