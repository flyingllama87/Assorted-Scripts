<# This script was used to find a bad setting/value in an internal application widely used in an office.  The value was causing issues for a remote host due to bad requests therefore the bogus values had to be found & cleaned up ASAP.

The script scans for a specific folder name on the local machine.  If that folder name is found, a particular file is checked for the bad value via RegEx match.  If a match is found for the bad value, information is printed and logged to a file.  

The script was executed and results gathered with the 'perform action on all PCs' script.
#>

# SCRIPT VARS
$PATH_TO_SEARCH_1 = "C:\"
$PATH_TO_SEARCH_2 = "D:\"
$PATH_TO_OUTPUT_FILES = "C:\TEMP_FILESCAN"
$BAD_PATTERN = '^[ \t]*\"productId\"[ \t]*\:[ \t]*9999,[ \t]*$'

# SETUP
$CURRENT_DATETIME = get-date -UFormat %d-%m_%R | foreach {$_ -replace ":", "-"} 											# The current date & time.
Remove-Item –path $PATH_TO_OUTPUT_FILES –recurse -EA SilentlyContinue 														# Remove previous results if they exist
New-Item -Path $PATH_TO_OUTPUT_FILES -ItemType directory -ErrorAction SilentlyContinue | out-null 							# Create output file directory if it doesn't already exist.
"BEGIN SCAN ON PC ($Env:COMPUTERNAME) AT $CURRENT_DATETIME" | out-file "$($PATH_TO_OUTPUT_FILES)\$($Env:COMPUTERNAME).txt" 	# Log start of scan

# MAIN FUNCTION
function FIND_BAD_ID {
    param([string]$PATH_TO_SEARCH)

    $CONFIG_DIRS = get-childitem -Directory -Recurse -ErrorAction SilentlyContinue -Path $PATH_TO_SEARCH | ?{ $_.Name -contains "PhantomConfig"}

    ForEach ($CONFIG_DIR in $CONFIG_DIRS.FullName) {
        If (Test-Path "$CONFIG_DIR\ConfigFile.json") {
            $CONFIG_FILE_PATH = "$($CONFIG_DIR)\ConfigFile.json"
            Write-Host "Looking in file $($CONFIG_FILE_PATH) on $($Env:COMPUTERNAME)"
            $CONFIG_FILE_CONTENT = get-content $CONFIG_FILE_PATH
            if ($CONFIG_FILE_CONTENT -match $BAD_PATTERN) {
                $CONFIG_FILE_PATH | out-file -append "$($PATH_TO_OUTPUT_FILES)\$($Env:COMPUTERNAME).txt"
                # Write-Host "Found a match in $($CONFIG_FILE_PATH) on $($Env:COMPUTERNAME)" -ForegroundColor Red
                Write-Host -ForegroundColor Red "Match: $($CONFIG_FILE_PATH)," `
                "$(get-content $CONFIG_FILE_PATH | select-string $BAD_PATTERN -All | out-string | % { $_.Trim() } )" `
                "ON $($Env:COMPUTERNAME)" 
            }
        }
    }
}

# SCAN THESE LOCATIONS
FIND_BAD_ID $PATH_TO_SEARCH_1
FIND_BAD_ID $PATH_TO_SEARCH_2


# SCRIPT TEARDOWN
$CURRENT_DATETIME = get-date -UFormat %d-%m_%R | foreach {$_ -replace ":", "-"} 											# The current date & time.
"END SCAN ON PC ($Env:COMPUTERNAME) AT $CURRENT_DATETIME" | out-file -Append "$($PATH_TO_OUTPUT_FILES)ACK\$($Env:COMPUTERNAME).txt"