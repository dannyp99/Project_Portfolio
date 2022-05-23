#!/bin/bash
if [[ -z $(which brew) ]] || [[ ! -f "/usr/local/bin/brew" ]];then
	echo "Please install brew, a package manager for Mac"
	echo "You can install it with:"
	echo "/bin/bash -c \"\$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install.sh)\""
	exit
fi

if [[ -d "$HOME/.config/omf" ]]; then
	if [[ -f "/etc/fonts/config.d/10-powerline-symbols.conf" ]]; then
		echo "Everything installed!"
		exit
	fi
fi
if [[ ! -f "/usr/local/bin/fish" ]]; then
  brew install fish
else
  echo "fish is already installed!"
fi
if [[ ! -d "$HOME/.config/omf" ]]; then
read -r -d '' myscript <<'EOF'
import sys
import os
for line in sys.stdin:
	if "successfully installed" in line:
		os.system("killall fish")
EOF
	fish -c "$(curl -L https://get.oh-my.fish)" | python3  -c "$myscript"
	echo "omf install bobthefish" | fish
else
	echo "omf is already installed!"
fi

if [[ ! -f "/usr/share/fonts/PowerlineSymbols.otf" ]]; then
	echo "Installing necessary fonts"
	su -c 'pip install git+git://github.com/Lokaltog/powerline'
	shell://github.com/Lokaltog/powerline/raw/develop/font/PowerlineSymbols.otf https://github.com/Lokaltog/powerline/raw/develop/font/10-powerline-symbols.conf
	open PowerlineSymbols.otf
	sudo mv PowerlineSymbols.otf /usr/share/fonts/
	sudo fc-cache -vf
fi

if [[ ! -f "/etc/fonts/config.d/10-powerline-symbols.conf" ]]; then
	sudo mv 10-powerline-symbols.conf /etc/fonts/conf.d/
fi
exit
