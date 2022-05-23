#!/bin/bash
if [[ $1 = "-h" ]] || [[ $1 = "--help" ]];then
	echo "-h,--help 				View all of the arguments for this script"
	echo "--font-manager			Used if you need to install font manager"
	echo "--install-font			Open the downloaded font file to install to system"
	exit
fi

if [[ -n $(which zsh) ]] && { [[ -f "/usr/bin/zsh" ]] || [[ -f "/bin/zsh" ]]; };then
	echo "zsh is already installed"
else 
	sudo apt -y install zsh
fi

if [[ -d "$HOME/.oh-my-zsh" ]];then
	echo "oh-my-zsh is already installed"
else
	cat <<< "n" | bash -c "$(curl -fsSL https://raw.githubusercontent.com/robbyrussell/oh-my-zsh/master/tools/install.sh)"
	if [[ ! -f "/usr/bin/killall" ]] && [[ -z $(which killall) ]]; then
		sudo apt -y install psmisc
	fi
	killall zsh
fi

if [[ -d "$HOME/.oh-my-zsh/custom/themes/powerlevel10k" ]];then
	echo "powerlevel10k is already installed"
else
	sudo git clone https://github.com/romkatv/powerlevel10k.git $HOME/.oh-my-zsh/custom/themes/powerlevel10k
fi

if [[ -f "/usr/share/fonts/Fira\ Mono\ Regular\ Nerd\ Font\ Complete.otf" ]] || [[ -f "/usr/local/share/fonts/Fira\ Mono\ Regular\ Nerd\ Font\ Complete.otf" ]] || [[ -f "$HOME/.fonts/Fira\ Mono\ Regular\ Nerd\ Font\ Complete.otf" ]];then
	echo "Fonts installed already"
else
	if [[ ! -f "/bin/wget" ]] && [[ -z $(which wget) ]];then
		sudo apt -y install wget
	fi
	if [[ ! -f "/usr/bin/curl" ]] || [[ -n $(which zsh) ]];then
		sudo apt -y install curl
	fi
	if [[ ! -f "$HOME/Downloads/Fira\ Mono\ Regular\ Nerd\ Font\ Complete.otf" ]] && { [[ $1 = "--font-manager" ]] || [[ $1 = "--install-font" ]]; };then
		wget -O $HOME/Downloads/Fira\ Mono\ Regular\ Nerd\ Font\ Complete.otf https://github.com/ryanoasis/nerd-fonts/blob/master/patched-fonts/FiraMono/Regular/complete/Fira%20Mono%20Regular%20Nerd%20Font%20Complete.otf?raw=true
	fi
	if [[ $1 = "--font-manager" ]] && [[ -z $(which font-manager) ]] && [[ ! -f /usr/bin/font-manager ]];then
		sudo apt -y install font-manager
		font-manager $HOME/Downloads/Fura\ Mono\ Regular\ Nerd\ Font\ Complete.otf
	elif [[ $1 = "--install-font" ]];then
		open $HOME/Downloads/Fura\ Mono\ Regular\ Nerd\ Font\ Complete.otf
	fi
fi

if [[ -d "$HOME/.oh-my-zsh/custom/plugins/zsh-syntax-highlighting" ]] && [[ -d "$HOME/.oh-my-zsh/custom/plugins/zsh-autosuggestions" ]];then
	echo "plugin already installed"
else 
	git clone https://github.com/zsh-users/zsh-syntax-highlighting.git $HOME/.oh-my-zsh/custom/plugins/zsh-syntax-highlighting
	git clone https://github.com/zsh-users/zsh-autosuggestions.git $HOME/.oh-my-zsh/custom/plugins/zsh-autosuggestions
	cp $HOME/.zshrc zshrc_backup
	echo "backup created in current directory as zshrc_backup"
	sed 's/ZSH_THEME.*/ZSH_THEME="powerlevel10k\/powerlevel10k"%POWERLEVEL9K_MODE="nerdfont-complete"/g' zshrc_backup | tr '%' '\n' > $HOME/.zshrc
	sed -i 's/plugins=.*/plugins=(git zsh-autosuggestions zsh-syntax-highlighting)/' $HOME/.zshrc
fi
echo "All Done Please refresh your your terminal or open zsh and type 'source ~/.zshrc'"
echo "If the fonts is not loaded config won't work. It should be in you $HOME/Downloads folder open it and select install!"
exit
