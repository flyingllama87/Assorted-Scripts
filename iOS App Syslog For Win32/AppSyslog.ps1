<#
This script was written for a QA team that needed to easily display and log information from iOS devices when testing an application.  This script allows the quality analyst to select the device they want to capture the log of (via network or USB cable) and initiate the capture.  

Only the syslog content related to the app is displayed but all syslog information is captured to a file with the device name & current date/time included.  This is because the entire syslog may benefit the development team when a bug is logged.

This script relied on the libimobiledevice executables being in the same folder:
- idevice_id.exe
- idevicename.exe
- idevicesyslog.exe

The advantage of this over executing idevicesyslog directly is that:
- It allows the user to easily select the device they want to capture the syslog of without passing the UDID as a commandline parameter
- Syslog is both displayed and logged to a file
- The list of devices that we can connect to is displayed as device name instead of UDID.

Can be easily launched from included BAT file which is useful for some users.
#>

# SETTINGS
$AppName = "MyAppName" 									# Name of the application being tested.
$OutputFileNamePrefix = "syslog"
$ErrorActionPreference = "Stop" 						# Exit if any errors are encountered.

# BEGIN

$CurrentDateTime = get-date -UFormat %d-%m_%R | foreach {$_ -replace ":", "-"}

$UDIDS = (.\idevice_id -l) 								# Get list of UDIDs of all found devices.

if ([string]::IsNullOrEmpty($UDIDS)) 					# If idevice_id did not provide any output, quit.
{
    write-host -ForegroundColor Red "No output from idevice_id found.  Is a device connected?"
    exit
}

$UDIDS = $UDIDS.trim()
$UDIDS = $UDIDS.split(" ")

$DeviceName = "" 										# Init string

if ($UDIDS.GetType().Name -eq "Object[]") 				# If idevice_id has returned a list of potential devices instead of one entry, get the first entry
{
    write-host -ForegroundColor Yellow "Found multiple devices."
	
	$counter = 0

	# Find attached device names and prompt user for device selection.
	foreach ($UDID in $UDIDS)
	{
		
		$DeviceName = (.\idevicename.exe -u $UDID)		# Resolve the device name from the UDID
		$DeviceName = $DeviceName.trim()
		
		Write-Host "Press $counter & the enter key for $DeviceName UDID: $($UDID)" 
		
		$counter += 1
	}
	
	$selection = Read-Host "Please make a selection"
	$selection = [int]$selection
	
	if (!($selection -le $UDIDS.GetUpperBound(0)) -or !($selection -ge $UDIDS.GetLowerBound(0)))
	{
		Write-Host -ForegroundColor Red "Invalid selection.  Selection is out of bounds." 
		exit
	}

	$UDID = $UDIDS[$selection]
	$DeviceName = (.\idevicename.exe -u $UDID)
	$DeviceName = $DeviceName.trim()
	
	write-host "You selected the device with UDID $UDID.  It has the device name of $DeviceName "
}

if ($UDID.GetType().Name -ne "String" -or ($UDID.Length -ne 40 -and $UDID.Length -ne 15)) # If we are getting an  unexpected length from the UDID variable, exit.  Anyhting more modern than the iPhone X will have a 15 digit UDID, older devices have a 40 digit UDID.  
{
    write-host -ForegroundColor Red "Error validating UDID: $($UDID) "
    exit
}

write-host -ForegroundColor Green "Found device $($DeviceName).  UDID: $($UDID)"

$Pattern = '[^a-zA-Z0-9]'  								# Set RegEx to clean up the name of the device in case it has non-printable characters. 
$DeviceName = $DeviceName -replace $Pattern,'' 

write-host "Will output full syslog to file: $($OutputFileNamePrefix)_$($DeviceName)_$($CurrentDateTime).txt"
write-host "Only $AppName related information will be displayed though"

.\idevicesyslog.exe -u $UDID  | tee-object $($OutputFileNamePrefix)_$($DeviceName)_$($CurrentDateTime).txt | select-string $AppName
