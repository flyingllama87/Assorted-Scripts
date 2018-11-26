#!/bin/bash

# Silent XCODE install script.
# Ensure's system meets minimum requirements and XCODE isn't running.  Then moves existing XCODE install to XCODE_OLD and copies/extracts/install newer XCODE version. 
# Requires execution as sudo

# SETTINGS
XCODE_INSTALL_ARCHIVE_FILE="Xcode_8.3.2.xip"
MINIMUM_MAJOR_VERSION=12
MINIMUM_MINOR_VERSION=5
MOUNT_FOLDER="/Volumes/XCODEINSTALL"
SMB_USERNAME=username
SMB_PASSWORD=password
SMB_PATH_XCODE_INSTALL="//${SMB_USERNAME}:${SMB_PASSWORD}@SERVER/Installs/OSX%20Installs"
# Optional command line tools.  Uncomment relevant lines in script to install.
COMMANDLINE_TOOLS_FILE_NAME="CommandLineToolsforXcode.dmg"


# Enable debug
set -x

MACOS_VERSION = "$(sw_vers -productVersion)"

MACOS_MAJOR_VERSION = $(MACOS_VERSION) | cut -d'.' -f 2
MACOS_MINOR_VERSION = $(MACOS_VERSION) | cut -d'.' -f 3

MACOS_REQUIREMENTS_MET = 0

if [ MACOS_MAJOR_VERSION -gt MINIMUM_MAJOR_VERSION ]
	then
	MACOS_REQUIREMENTS_MET = 1
fi

if [ MACOS_MAJOR_VERSION -eq MINIMUM_MAJOR_VERSION ] && [ MACOS_MINOR_VERSION -ge MINIMUM_MINOR_VERSION ]
	then
	MACOS_REQUIREMENTS_MET = 1
fi

if [ MACOS_REQUIREMENTS_MET != 1 ]
	then
	echo "YOU NEED TO UPDATE MacOS TO THE MINIMUM VERSION!  QUITTING..."
	exit 1
fi

if [ "$(ps ax | grep -c '[X]code.app')" -ge 1 ]
    then
	echo "ERROR: Xcode running or xcode.app folder in use. Perform process listing and kill -9 the process and rerun to continue."
	echo "RUNNING ps ax FOR YOU TO VIEW DETAILS"
	ps ax | grep '[X]code.app'
	
	# Auto xcode kill 
	# echo "xcode process open... attempting to kill process"
	# PID_TO_KILL=$(ps ax | grep '[X]code.app' | cut -f1 -d ' ')
	# kill -9 ${PID_TO_KILL}
	# sleep 5
	# if [ "$(ps ax | grep -c '[X]code.app')" -ge 1 ]
	#   then
	#    echo "Couldn't Kill XCODE. Quitting..."
	#	exit 1
	#fi
fi
	
# Mount SMB Share
mkdir $MOUNT_FOLDER || echo "mkdir failed.  Does $MOUNT_FOLDER already exist?"
mount -t smbfs $SMB_PATH_XCODE_INSTALL $MOUNT_FOLDER || echo "Mount failed. $MOUNT_FOLDER already mounted?"
	
# Copy Xcode over locally
cp -f $MOUNT_FOLDER/$XCODE_INSTALL_ARCHIVE_FILE ./$XCODE_INSTALL_ARCHIVE_FILE
    
# Copy pbzx needed to handle xip archives on the commandline.
cp -f $MOUNT_FOLDER/pbzx /usr/local/bin/pbzx
chmod a+x /usr/local/bin/pbzx || echo "not able to make pbzx executable"
    
# Extract Apple's XCODE XIP archive
pkgutil --check-signature $XCODE_INSTALL_ARCHIVE_FILE && xar -xf $XCODE_INSTALL_ARCHIVE_FILE && pbzx Content | tar x --strip-components=1
rm "Content" "Metadata"

# Remove any really old XCODE versions previously not removed.
rm -rf /Applications/Xcode_old.app

# Keep old XCODE & move new one in place.
mv /Applications/Xcode.app /Applications/Xcode_old.app || echo "Xcode not installed"
mv ./Xcode.app /Applications/Xcode.app
    
sleep 3
rm ./$XCODE_INSTALL_ARCHIVE_FILE

# Developer tools install.  Generally not needed.
# hdiutil attach $MOUNT_FOLDER/$COMMANDLINE_TOOLS_FILE_NAME
# installer -pkg "/Volumes/Command Line Developer Tools/Command Line Tools (macOS Sierra version 10.12).pkg" -target /
# hdiutil detach "/Volumes/Command Line Developer Tools" || echo "Could not detach developer tools"
    
# Close SMB Share
sleep 5
diskutil umount $MOUNT_FOLDER || echo "Could not unmount $MOUNT_FOLDER"

# Run XCODE post copy/install packages
/Applications/Xcode.app/Contents/Developer/usr/bin/xcodebuild -license accept
installer -pkg /Applications/Xcode.app/Contents/Resources/Packages/MobileDevice.pkg -target /
installer -pkg /Applications/Xcode.app/Contents/Resources/Packages/MobileDeviceDevelopment.pkg -target /
installer -pkg /Applications/Xcode.app/Contents/Resources/Packages/XcodeSystemResources.pkg -target /
    
echo "XCODE UPGRADE SUCCESS! You can remove the old xcode (Xcode_old.app) in the applications folder!"

set +x