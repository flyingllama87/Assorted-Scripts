<#
This script retrieves the total login time from PCs for the user assigned to that PC for the day before the script is executed.  Results are exported to a CSV.

The script first generates a list of computers to contact by concatenting a computername prefix with a sequentially incrementing number (E.G. WKS002, WKS003, etc.).  It'll then work out which computers are contactable and then attempts to retrieve the data from the PC.

The script calculates the total login time by checking the first logon or unlock event of the day and the last lock, logoff or RDP disconnect event event of the day and finally calculating the difference between the two.

Notes: 
- Only PCs that are on are processed.
- Local admin access is required on the PCs.  Credentials are asked for on script execution.
- Computer to user mapping is stored in a csv ($COMPUTER_TO_USER_MAP_FILE).  There should be a 'COMPUTER_NAME' & 'USERNAME' column (with headings).
- This script may have privacy and ethtical implications that should be considered before script execution.  Make sure you have relevant approval.
- I have seen Excel automatically hide the hours in the 'TOTAL TIME LOGGED IN' column when opening the generated CSV.  Make sure to adjust the format of this column if data doesn't look right.
- The data is simply the first & last logon times.  Not the total active time.

A key improvement would be to either use the first column of the PC to user mapping CSV for the list of computer names or to not require a PC to user mapping.

#>

# SCRIPT SETTINGS

$COMPUTER_TO_USER_MAP_FILE = "COMPUTER_TO_USER_MAP.csv"
$COMPUTER_NAME_PREFIX = "WKS"
$TOTAL_NUMBER_OF_WORKSTATIONS = 150 													# Generate a list of computer names up to this number.  Up to 999.
$TODAYS_DATE = $(get-date).date 														# Strip time from date.
$DATE_TO_SCAN = $TODAYS_DATE.AddDays(-1) 												# This pulls records for the previous day.  Override with specific date on next line if getting data for specific date.
# $DATE_TO_SCAN = get-date -date "25/12/2011"											# Use this line if you want to get records for a specific date
$OUTPUT_PATH = "D:\OUTPUT\"
$OUTPUT_FILE = "$($OUTPUT_PATH)\TOTAL_LOGON_TIME_$($DATE_TO_SCAN.DAY)_$($DATE_TO_SCAN.MONTH).csv"
# $OUTPUT_FILE_NAME_CONTACTABLE = "CONTACTABLE_COMPUTERS"
# $OUTPUT_FILE_NAME_NON_CONTACTABLE = "NON_CONTACTABLE_COMPUTERS"

$LOGOFF_EVENT_IDS = 4800,4647,4634 														# Logoff, lock and RDP disconnections
$LOGON_EVENT_IDS = 4801,4624 															# Unlock or log on

$CURRENT_DATETIME = get-date -UFormat %d-%m_%R | foreach {$_ -replace ":", "-"} 		# The current date & time is appended to all generated lists in a filename friendly manner.

if (!$CREDENTIAL) {$CREDENTIAL = get-credential} 										# Get credential if it's not already set in user shell

# Declare lists
$LIST_OF_COMPUTERS_TO_SCAN = @()
$CSV_COMPUTER_NAME = @()
$CSV_USERNAMES = @()

Import-Csv $OUTPUT_PATH\$COMPUTER_TO_USER_MAP_FILE |`
    ForEach-Object {
        $CSV_COMPUTER_NAME += $_.COMPUTER_NAME
        $CSV_USERNAMES += $_.USERNAME
    }

# Generate list of workstations
for ($i=1; $i -le $TOTAL_NUMBER_OF_WORKSTATIONS; $i++) {


    $COMPUTERNAME = $COMPUTER_NAME_PREFIX

	if ($i -ge 10 -and $i -le 99)														# Pad computer name with extra 0 if current number is only two digits long.
    {
        $COMPUTERNAME = $COMPUTERNAME + "0"
    }
	
    if ($i -le 9)																		# Pad computer name with extra two 00s if current number is only one digit long.
    {
        $COMPUTERNAME = $COMPUTERNAME + "00"
    }
	
	$COMPUTERNAME = $COMPUTERNAME + $i													# Generate computer name with number appended.

	if (Test-NetConnection $COMPUTERNAME -InformationLevel Quiet)						# Check for connectivity
    {
        # $COMPUTERNAME | out-file -append "$OUTPUT_PATH\$OUTPUT_FILE_NAME_CONTACTABLE_$CURRENT_DATETIME.TXT" 		# Write current computer name in list of contactable computers.
		$LIST_OF_COMPUTERS_TO_SCAN += $COMPUTERNAME
    }
    else
    {
        # $COMPUTERNAME | out-file -append "$OUTPUT_PATH\$OUTPUT_FILE_NAME_NON_CONTACTABLE_$CURRENT_DATETIME.TXT"		# Write current computer name in list of non contactable computers.
    }
}

# Create CSV file & dump header
"`"USERNAME`",`"COMPUTERNAME`",`"TOTAL TIME LOGGED IN`",`"LOGON TIME`",`"LOGOFF TIME`""  | out-file $OUTPUT_FILE -Encoding ASCII

Foreach ($COMPUTER in $LIST_OF_COMPUTERS_TO_SCAN)
{
	# Reset $TIME_OF_LOGOFF var
	$TIME_OF_LOGOFF = ""
	$TIME_LOGGED_ON = ""
	
	# Modify the size of the 'security' event log so future data is stored for a longer period.
	# Invoke-Command -Computer $COMPUTER -Credential $CREDENTIAL -ScriptBlock {Limit-Eventlog -Logname Security -MaximumSize 100MB -OverflowAction OverwriteAsNeeded}
	
	# Get username for this computer from user <-> computer name CSV 
	$RECORD_INDEX = [array]::IndexOf($CSV_COMPUTER_NAME, $COMPUTER)
	$USERNAME = $CSV_USERNAMES[$RECORD_INDEX]
	
	# Use éxplorer process to determine currently loggon on user.  Alternate to using the mapping file but was not suitable in all scenarios.
	# $USERNAME = (Get-WmiObject -Class win32_process -ComputerName $COMPUTER | Where-Object name -Match explorer).getowner().user | select -first 1
	
	write-host "NOW SCANNING $($COMPUTER) ($($USERNAME)) at $($(get-date).TimeOfDay)"
	
	# ACQUIRE LOGIN EVENT TIME
	$LIST_OF_LOGON_EVENTS = Get-WinEvent -ComputerName $COMPUTER -FilterHashTable @{logname='Security'; id=$LOGON_EVENT_IDS; StartTime=$DATE_TO_SCAN; EndTime=$DATE_TO_SCAN.AddDays(1); Data=$USERNAME} | Sort -Property TimeCreated
	if (!$LIST_OF_LOGON_EVENTS) { continue } # Skip this record if the user did not log on.
	$FIRST_LOGON_EVENT = $LIST_OF_LOGON_EVENTS | select -first 1 
	$TIME_OF_LOGON = $FIRST_LOGON_EVENT.TimeCreated

	# ACQUIRE LOGOFF EVENT TIME
	$LIST_OF_LOGOFF_EVENTS = Get-WinEvent -ComputerName $COMPUTER -FilterHashTable @{logname='Security'; id=$LOGOFF_EVENT_IDS; StartTime=$DATE_TO_SCAN; EndTime=$DATE_TO_SCAN.AddDays(1); Data=$USERNAME} | Sort -Property TimeCreated
	$LAST_LOGOFF_EVENT = $LIST_OF_LOGOFF_EVENTS | select -last 1
	$TIME_OF_LOGOFF = $LAST_LOGOFF_EVENT.TimeCreated
	
	$TIME_LOGGED_ON = New-TimeSpan -Start $TIME_OF_LOGON -End $TIME_OF_LOGOFF
	
	# Generate CSV entry and write.
	$CSV_STR_TO_WRITE = "`"$($USERNAME)`",`"$($COMPUTER)`",`"${$TIME_LOGGED_ON.ToString()}`",`"$($TIME_OF_LOGON)`",`"$($TIME_OF_LOGOFF)`""
	Write-host $CSV_STR_TO_WRITE
	$CSV_STR_TO_WRITE | out-file -append $OUTPUT_FILE -Encoding ASCII
	
}

Write-host "Script Complete at ${get-date}"


