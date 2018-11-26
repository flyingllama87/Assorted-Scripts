<#
This script queries the Cisco Meraki MDM REST API for all devices and the datetime of their last check-in.  If the device has not checked in in the period listed, set a bad status.  Designed to be sent and read from a monitoring system.
#>
 
# SETTINGS
$NetworkID = "N_999999999999999999" 										# Meraki Network ID
$MerakiAPIKey = "ffffffffffffffffffffffffffffffffffffffff"					# Authentication key
$DeviceIgnoreList = "DEVICENAME1", "DEVICENAME2", "DEVICENAME3" 			# Device to ignore due to various circumstance (e.g. under repair).
$CheckInTimeGracePeriod = 1209600 											# 2 weeks.  This is the period of time a phone can not have checked in before an alert is sent.

# strings that need to be output for monitoring system.
$statusOk = "ScriptRes:Ok:"
$statusBad = "ScriptRes:Bad:"

# Set/unset a proxy if needed.
# $env:http_proxy = ""
# $env:https_proxy = ""
# [system.net.webrequest]::defaultwebproxy = $null
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12 
 
$CurrentDateTimeUnixFmt = Get-Date
$CurrentDateTimeUnixFmt = [int]([DateTimeOffset]$CurrentDateTimeUnixFmt).ToUnixTimeSeconds()

$Headers = @{}
$Headers.Add("Content-Type", "application/json")
$Headers.Add( "X-Cisco-Meraki-API-Key", $MerakiAPIKey)

# Poll API for list of devices and ask for their ID, name and their last connection date.
$Result = Invoke-WebRequest -Method GET -Uri "https://api.meraki.com/api/v0/networks/$NetworkID/sm/devices?fields=id,name,lastConnected" -Headers $Headers

# View raw response from API
# write-host $Result 

$Result = $Result | ConvertFrom-Json
$Result = $Result.devices

foreach ($DeviceEntry In $Result)
{
	$Name = $DeviceEntry.name
	$LastConnectTime = $DeviceEntry.lastConnected
	$DeviceType = $DeviceEntry.systemModel

	# View device information
	# Write-Host $Name
	# Write-Host $LastConnectTime

	if ($LastConnectTime + $CheckInTimeGracePeriod -lt $CurrentDateTimeUnixFmt -and $Name -notin $DeviceIgnoreList)
	{
		write-host -ForegroundColor Red $statusBad " Error! $Name ($DeviceType) has not checked-in in time! $($LastConnectTime) + $($CheckInTimeGracePeriod) ($($LastConnectTime + $CheckInTimeGracePeriod)) is less than $($CurrentDateTimeUnixFmt). Exiting!"
		exit 1
	}
}

# If script execution made it to here, all devices are good.
write-host $statusOk
exit 0
