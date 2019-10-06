#!/bin/bash
b=/bin/bash
dis="undef"
perm="undef"
packages=(python3 git gcc-avr avr-libc avrdude subversion autoconf libtool automake pkg-config libao-dev zlib1g-dev libcurl4-gnutls-dev)

if [ -f /etc/issue ]
	then
		distro=$(cat /etc/issue | awk '{ print $1; };')
		for e in $(cat /etc/issue)
			do
				if [[ $distro == *"ebian"* && ( $e == *"7." || $e == *"8."* || $e == *"9."* ) ]] || [[ $distro == *"buntu"*  && ( $e != *"10."* || $e != *"12."* ) ]] 
					then
						perm="accept"
				fi

			done
fi


if [[ $distro == *"buntu"* ]]
	then
		dis="ub"
		printf "Detected Ubuntu system!\n"
elif [[ $distro == *"ebian"* ]]
	then
		dis="deb"
		printf "Detected Debian system!\n"
fi



if [[ $perm == "accept" && $dis != "undef" ]]
	then
		for pkg in $(dpkg --get-selections | awk '{ print $1; }')
			do
				for (( i=0;i<${#packages[@]};i++ ))
					do
						if [[ ${packages[$i]} == *"$pkg"*  ]]
							then
#								packages[$i]=""
								printf ""
						fi
					done
			done
echo "Installing required packages"
sleep 3
apt-get install -y ${packages[@]} > /dev/null 2>>log.txt
apt-get install -y build-essential > /dev/null 2>>log.txt
echo "Downloading sc68"
sleep 3
svn checkout http://svn.code.sf.net/p/sc68/code/ sc68 > /dev/null 2>>log.txt
cd sc68
d=$(pwd)
echo "Running SVN-BOOTSTRAP.SH"
$b ${d}/tools/svn-bootstrap.sh > /dev/null 2>>log.txt
echo "Running CONFIGURE"
$b configure > /dev/null 2>>log.txt
echo "Running MAKE"
sudo make > /dev/null 2>>log.txt
echo "Running MAKE INSTALL"
sudo make install > /dev/null 2>>log.txt
ressc68=$(sc68)
if [[ ressc68 != *"sc68: missing input file. Try --help."* ]]
	then
		sudo cp /usr/local/lib/lib* /usr/lib/
fi
echo "Successfully installed SC68"
sleep 3

cd ..
echo "Now cloning YM2149-SNDH"
sleep 3
git clone https://github.com/FlorentFlament/ym2149-sndh.git > /dev/null 2>/log.txt
cd ym2149-sndh
echo "Installing YM2149-SNDH"
make > /dev/null 2>>log.txt
if [[ $? == 0 ]]
	then
		printf "SUCCESS!\n"
		cd ym2149-sndh
	else
		printf "Error...\n"
fi


else
	printf "Error! Unsupported system/distro!\n"
	exit 1
fi
