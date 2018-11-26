<#
This simple script generates a list of computer names that have a sequentially incrementing number (E.G. WKS002, WKS003, etc.) and then tries to contact those machines.   It will dump three lists / TXT files:
- one that lists all computers
- one that lists all computers that can be contacted
- one that lists all computers that can not be contacted.

The current date & time will be appended to the name of the text files.

Parameters modified in first variables listed.

#>
 
$OUTPUT_PATH = "D:\OUTPUT\"
$OUTPUT_FILE_NAME_WORKSTATION_LIST = "COMPUTER_LIST"
$OUTPUT_FILE_NAME_CONTACTABLE = "CONTACTABLE_COMPUTERS"
$OUTPUT_FILE_NAME_NON_CONTACTABLE = "NON_CONTACTABLE_COMPUTERS"

$COMPUTER_NAME_PREFIX = "WKS"
$TOTAL_NUMBER_OF_WORKSTATIONS = 150 													# Generate a list of computer names up to this number.  Up to 999.

$CURRENT_DATETIME = get-date -UFormat %d-%m_%R | foreach {$_ -replace ":", "-"} 		# The current date & time is appended to all generated lists in a filename friendly manner.

# Generate lists of workstations
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
	
    $COMPUTERNAME | out-file -append "$OUTPUT_PATH\$($OUTPUT_FILE_NAME_WORKSTATION_LIST)_$CURRENT_DATETIME.TXT"		# Write current computer name in list of all computers.

	
	if (Test-NetConnection $COMPUTERNAME -InformationLevel Quiet)						# Check for connectivity
    {
        $COMPUTERNAME | out-file -append "$OUTPUT_PATH\$OUTPUT_FILE_NAME_CONTACTABLE_$CURRENT_DATETIME.TXT" 		# Write current computer name in list of contactable computers.
    }
    else
    {
        $COMPUTERNAME | out-file -append "$OUTPUT_PATH\$OUTPUT_FILE_NAME_NON_CONTACTABLE_$CURRENT_DATETIME.TXT"		# Write current computer name in list of non contactable computers.
    }
}
