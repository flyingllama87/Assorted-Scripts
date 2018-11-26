<#
This simple script generates a list of computer names that have a sequentially incrementing number (E.G. WKS002, WKS003, etc.), work out which computers are contactable and then attempts to execute commands on them via a ScriptBlock.

It will dump three lists / TXT files:
- one that lists all computers that can be contacted
- one that lists all computers that can not be contacted.
- one that lists all computers where an execution error occured.

The current date & time will be appended to the name of the text files.

This script may depend on computers having an unrestricted execution policy.

Parameters modified in first variables listed.

#>
 
$OUTPUT_PATH = "D:\OUTPUT\"
$OUTPUT_FILE_NAME_CONTACTABLE = "CONTACTABLE_COMPUTERS"
$OUTPUT_FILE_NAME_NON_CONTACTABLE = "NON_CONTACTABLE_COMPUTERS"
$OUTPUT_FILE_NAME_EXECUTE_ERROR = "COMPUTER_LIST_EXECUTE_ERROR"

$COMPUTER_NAME_PREFIX = "WKS"
$TOTAL_NUMBER_OF_WORKSTATIONS = 150 													# Generate a list of computer names up to this number.  Up to 999.

$CURRENT_DATETIME = get-date -UFormat %d-%m_%R | foreach {$_ -replace ":", "-"} 		# The current date & time is appended to all generated lists in a filename friendly manner.

if (!$CREDENTIAL) {$CREDENTIAL = get-credential} 
$LIST_OF_COMPUTERS_TO_SCAN = @()


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
        $COMPUTERNAME | out-file -append "$OUTPUT_PATH\$OUTPUT_FILE_NAME_CONTACTABLE_$CURRENT_DATETIME.TXT" 		# Write current computer name in list of contactable computers.
		$LIST_OF_COMPUTERS_TO_REMOTELY_ACCESS += $COMPUTERNAME
    }
    else
    {
        $COMPUTERNAME | out-file -append "$OUTPUT_PATH\$OUTPUT_FILE_NAME_NON_CONTACTABLE_$CURRENT_DATETIME.TXT"		# Write current computer name in list of non contactable computers.
    }
}

# Loop over every contactable computer to run commands on it. 
Foreach ($COMPUTERNAME in $LIST_OF_COMPUTERS_TO_REMOTELY_ACCESS)
{
	Invoke-Command -Computer $COMPUTER -Credential $CREDENTIAL -ScriptBlock { 
		# Executed on remote computer.  Example command is to print out the local computername.
		
		try
		{
			write-host $Env:COMPUTERNAME 
		}
		catch
		{
			# command failed: log, clean up etc.
			return $_
		}
	}
	
	if ($?) # If the ScriptBlock resulted in an error... 
	{
		$COMPUTERNAME | out-file -append "$OUTPUT_PATH\$OUTPUT_FILE_NAME_EXECUTE_ERROR_$CURRENT_DATETIME.TXT"		# Write current computer name in list of computers with execution error
	}
}
