#!/bin/bash

# For MacOS.  Requires Xcode to be installed / PlistBuddy to be available in /usr/libexec/
# Written by Morgan R. 
# Script is handed an IPA as the first argument and will then:
# - Perform some simple checks
# - Mount the OTA servers shares
# - Make a new folder for the IPA (It finds the last build number, adds one to it and uses this number for the folder)
# - Copy the IPA over
# - Generate the requisite files for the web server (index.php) and those required by iOS (plist)
# 


# SETTINGS
TITLE=MyGameName
SMB_USERNAME=username
SMB_PASSWORD=password
OTA_SERVER_NAME=otaserver.internal.company.org
OTA_SMB_SHARE_NAME=Builds
SMB_PATH_TO_MOUNT="//${SMB_USERNAME}:${SMB_PASSWORD}@${OTA_SERVER_NAME}/${OTA_SMB_SHARE_NAME}/${TITLE}"
OTA_BASE_URL="https://${OTA_SERVER_NAME}/${OTA_SMB_SHARE_NAME}/"



# INTERNAL SETTINGS
IPA_FILE=$1
CURRENT_DIRECTORY=$(pwd)

	
# Check if a file with the IPA extension was provided.  Exit if the ipa string was not found.
if [ $( echo "$1" | grep -i -v 'IPA$' ) ]
   then
    echo "**ERROR**: IPA file NOT was provided"
    exit 1
fi

echo "The file provided is: $IPA_FILE"
echo " " #Blank link for cleanliness

# The next few commands extract the IPA file to check if it's an actual IPA file and also retrieve the required IPA information for OTA deployment.

# Clean up previous run
rm -f $TITLE.zip
rm -rf ./Payload

# Rename the IPA file to $TITLE.zip
mv $1 $TITLE.zip

# unzip the file 
unzip -q $TITLE.zip

# CD into payload folder
cd Payload

# CD into any extracted .app folder
cd *.app

#Check to see if Info.plist exists
if [ ! -f Info.plist ]
  then
   echo "**ERROR**: Info.plist does not exist in current folder:"
   echo " " #Blank link for cleanliness
   pwd
   exit 1
fi

# Dump the required information for the OTA Plist file from the embedded Info.plist file and store into vars.
BUNDLE_IDENTIFIER=$(/usr/libexec/PlistBuddy -c "Print :CFBundleIdentifier" ./Info.plist)
BUNDLE_VERSION=$(/usr/libexec/PlistBuddy -c "Print :CFBundleShortVersionString" ./Info.plist)

echo "Bundle ID: $BUNDLE_IDENTIFIER"
echo "Bundle Ver: $BUNDLE_VERSION"
echo " " #Blank line for cleanliness


cd "$CURRENT_DIRECTORY"
#Mount SMB Share, continue even if errors are encountered, change into it
mkdir "./${TITLE}_OTA" || echo "**WARNING**: mkdir failed.  Does ./${TITLE}_OTA already exist?"
mount -t smbfs ${SMB_PATH_TO_MOUNT} "./${TITLE}_OTA" || echo "**WARNING**: Mount failed. Is ./${TITLE}_OTA already mounted?"
cd ./${TITLE}_UNITY_OTA/

#List only folders in list format, then pipe to trim to remove trialling forward slash, then pipe to sort for list sorted by highest number first, then pipe to head to grab the top number
LAST_BUILD_NUMBER=$(ls -1 -d */ | tr -d '/' | sort -n -r | head -n 1)

#Take the last build number and increment by one
NEW_BUILD_NUMBER=$((LAST_BUILD_NUMBER+1))

# Make directory for new build and CD into it.
mkdir $NEW_BUILD_NUMBER
cd $NEW_BUILD_NUMBER

echo "New Build Number will be: $NEW_BUILD_NUMBER"
echo " " #Blank link for cleanliness

# Copy the provided IPA to the build server
cp ${IPA_FILE} ./${TITLE}.ipa

# Some setups may require permissions to be set.
# chmod 640 ./${TITLE}.ipa

# Construct URL of the IPA file on the server
IPA_URL=${OTA_BASE_URL}/${TITLE}/$NEW_BUILD_NUMBER/${TITLE}.ipa

echo "Build will be available at ${OTA_BASE_URL}/${TITLE}/$NEW_BUILD_NUMBER/"
echo " " #Blank link for cleanliness

# Generate the plist file used by iOS that points to the IPA to be installed on to the device.  BUNDLE_IDENTIFIER and BUNDLE_VERSION must match the IPA.
cat << EOF > $TITLE.plist
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>items</key>
    <array>
        <dict>
            <key>assets</key>
            <array>
                <dict>
                    <key>kind</key>
                    <string>software-package</string>
                    <key>url</key>
                    <string>$IPA_URL</string>
                </dict>
            </array>
            <key>metadata</key>
            <dict>
                <key>bundle-identifier</key>
                <string>$BUNDLE_IDENTIFIER</string>
                <key>bundle-version</key>
                <string>$BUNDLE_VERSION</string>
                <key>kind</key>
                <string>software</string>
                <key>title</key>
                <string>$TITLE</string>
            </dict>
        </dict>
    </array>
</dict>
</plist>
EOF


if [ $? ]
  then
   echo "**ERROR**: Can't create $TITLE.plist"
   echo " " #Blank link for cleanliness
   exit 1
fi

# Generate the PHP file for the OTA server that points to the above plist file for the IPA being deployed.
cat << EOF > index.php
<html>
 <head>
  <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
  <title>Builds</title>
  <?php include \$_SERVER['DOCUMENT_ROOT']."/style.php"; ?>
 </head>
 <body>
  <?php include \$_SERVER['DOCUMENT_ROOT']."/navbar.php"; ?>
  <br>
  <div class="container">
   <div class="jumbotron">
    <h1>Builds</h1>
    <p><small>Built packages for ${TITLE}.</small></p>
   </div>
   <div class="panel panel-primary">
   <br>
   <div class="container">
    <a role="button" class="btn btn-lg btn-primary" href="itms-services://?action=download-manifest&amp;url=${OTA_BASE_URL}${TITLE}/${NEW_BUILD_NUMBER}/${TITLE}.plist">
	   <h1>Install ${TITLE} Build</h1>
    </a>
   </div>
   <br>
   </div>
  </div>
 </body>
</html>
EOF
	
if [ $? ]
  then
   echo "**ERROR**: Can't create index.php"
   echo " " #Blank link for cleanliness
   exit 1
fi

# Return to initial directory
cd $CURRENT_DIRECTORY
	
# Close SMB Share
sleep 5
diskutil umount ./${TITLE}_OTA || echo "**WARNING**: Could not unmount ./${TITLE}_OTA"
sleep 2
# Try to rmdir.  Do not rm -rf or force removal in case the above unmount failed.  rmdir will fail if files exist (e.g. umount failed).
rmdir ./${TITLE}_OTA || echo "Can't rmdir ./${TITLE}_OTA "

echo " " # Blank line for cleanliness
echo "Complete!"
echo " " # Blank line for cleanliness
