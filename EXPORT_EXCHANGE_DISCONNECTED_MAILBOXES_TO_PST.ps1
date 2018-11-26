###############################################################################################################################
# Purpose:  Export all disconnected mailboxes to PST files.  Run in Exchange Management Shell.                                #
# 
# The script gets a list of all disconnected mailboxes for a specified server and stores their display names in a variable.   #
# It then connects the disconnected mailboxes (one a time) to an enabled AD user, queues the export request and waits for the # 
# export to complete before disconnecting the mailbox and continuing to the next user.  The display name is used for the      #
# filename & the name of the export request.                                                                                  #
#                                                                                                                             #
# I'd recommend using Clean-MailboxDatabase before running the script to ensure all disconnected mailboxes are listed.        #
# You can also purge disconnected mailboxes after exporting with Remove-StoreMailbox.                                         #
###############################################################################################################################

$Mail_server = "MAILSRV1.COMPANY.INTERNAL"
$Database_name = "MBXDB1" 
$UNC_PSTPath = "\\filesrv\IT\Backups\ArchivePSTs" #Exclude slash
$Mailbox_Archive_username = "mailboxarchive"
$Mailbox_Archive_DisplayName = "Mailbox Archiver"


# Get list of disconnected mailboxes from server:
$mailboxes = Get-MailboxStatistics -server $Mail_server | where { $_.DisconnectDate -ne $null } | select DisplayName

#Loop through each disconnected mailbox and save each object into 'mailbox' variable
foreach ($mailbox in $mailboxes) {

	#If the disconnected mailbox already has a DisplayName of the Mailbox Archiver AD account, It's already been processed, so skip it.
	if ($mailbox.DisplayName -ne $Mailbox_Archive_DisplayName) { 
		
		# Connect Mailbox to mailbox archive AD user.
		Write-Host "Connecting Mailbox: " $mailbox.DisplayName
		Connect-Mailbox -Identity $mailbox.DisplayName -Database $Database_name -User $Mailbox_Archive_username

		#Replace commas in safename variable.  If the display name for your users have any other non-alphanumeric characters, it is suggested they are replaced here.
		$safename = $mailbox.DisplayName -replace ','

		Write-Host Writing file to $UNC_PSTPath\$safename`.pst
		
		#Initialise a variable to skip the 120 second sleep when in PST export loop once only.
		$SkipSleep = "True"
		
		# Keep running the export command every 120 seconds until it's successful
		do {
			
			#Find out if the skipsleep variable is set and if not, sleep for 120 seconds.
			if ("True" -ne $SkipSleep) {
				Start-sleep -s 120
			}
			
			#Set SkipSleep variable so no more sleeping
			$SkipSleep = "False"
			
			#Initialise ExportError Variable.
			$ExportError = $False
						
			Try {
				# Expost Mailbox to PST.  The filename variable is used for the name of the export request/job in addition to the actual filename.
				New-MailboxExportRequest -Mailbox "$Mailbox_Archive_DisplayName" -FilePath "$UNC_PSTPath\$safename.pst" -Name $safename -MRSServer $Mail_server -ErrorAction Stop
			}
			Catch {
				Write-Host "Export Request Failed! Retrying..."
				$ExportError = $true
			}
			
			Write-Host "Export Error:" $ExportError
			
		} while ($ExportError -eq $true)
			
		Write-Host "File Export Queued.  Going into job monitoring mode..."
		
		#Initialise a variable to skip the 120 second sleep when in monitoring loop once only.
		$SkipSleep = "True"
		
		do {
			
			#Find out if we are going to sleep
			if ("True" -ne $SkipSleep) {
				Start-sleep -s 120
			}
			
			#Set SkipSleep variable
			$SkipSleep = "False"
			
			#Check the status of the export, as the filename variable is used for the job name as well, it is passed too.
			$ExportStatus = Get-MailboxExportRequest | where {$_.Name -eq $safename } | select Status
			
			Write-Host "Export Status:" $ExportStatus.Status
			
		} While ($ExportStatus.Status -ne "Completed")
			

		# Disable mailbox:
		Write-Host "Disabling mailbox for :" $safename
		Disable-Mailbox -Identity "$Mailbox_Archive_DisplayName" -Confirm:$False
		
	}
	Else {
		#Let user know if a user is being skipped.
		Write-Host "Skipping user as already processed"
	}

}