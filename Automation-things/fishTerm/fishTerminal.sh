#!/bin/bash
sudo apt update
sudo apt upgrade
if [ -d "$HOME/.config/omf" ]; then
	if [ -f "/etc/fonts/config.d/10-powerline-symbols.conf" ]; then
		echo "Everything installed!"
		exit
	fi
fi
if [ ! -f "/usr/bin/fish" ]; then
  sudo apt install fish
else
  echo "fish is already installed!"
fi
if [ ! -d "$HOME/.config/omf" ]; then
read -r -d '' myscript <<'EOF'
import sys
import os
for line in sys.stdin:
	if "successfully installed" in line:
		os.system("killall fish")
EOF

if [ ! -f "/usr/bin/curl" ]; then
	sudo apt install curl
fi

if [ ! -f "/usr/bin/killall" ]; then
	sudo apt install psmisc
fi
	fish -c "$(curl -L https://get.oh-my.fish)" | python3  -c "$myscript"
	echo "omf install bobthefish" | fish
else
	echo "omf is already installed!"
fi

if [ ! -f "/usr/share/fonts/PowerlineSymbols.otf" ]; then
	su -c 'pip install git+git://github.com/Lokaltog/powerline'
	shell://github.com/Lokaltog/powerline/raw/develop/font/PowerlineSymbols.otf https://github.com/Lokaltog/powerline/raw/develop/font/10-powerline-symbols.conf
	sudo mv PowerlineSymbols.otf /usr/share/fonts/
	sudo fc-cache -vf
fi

if [ ! -f "/etc/fonts/config.d/10-powerline-symbols.conf" ]; then
	sudo mv 10-powerline-symbols.conf /etc/fonts/conf.d/
fi
exit
