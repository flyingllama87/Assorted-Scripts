# This script was designed to make WIFI management easier in an environment that uses 802.1X certificate authentication and a custom web interface (Wifi Web Tool / WWT) to generate the certificates.
# 
# Although the script is environment specific due to the reliance of the custom WWT.  It's an example of some of the scripts I've written.  In this instance, it's for a tool that does not have an API available for use so the selenium is used to automate the webtool.  The advantage of this is that any web page can be automated.  The downside is that changes to the web interface may break automation scripts.
#
# Ideally, a REST API could be used for the webtool automation and an MDM library could be used to automatically install certificates to attached mobile devices.
#
# The script will generate a new WIFI certificate for every device listed in the CSV passed to it using the ‘--AddDevices’ switch.
# When executed, the script should start your browser via selenium and automate the webtool.  All actions are logged to both file and to user.
# You can optionally pass the --GenerateHTML option to generate a HTML/php page that can then be hosted on a webserver.  The purpose of this is to make it easy to install the certificates on a device.
# See usage or source for more.
#
# I tried to make this script readable with a good set of usage instructions and help text.
#
# Requires Python 3 & the following libraries: selenium, argparse, HTMLParser, urllib, pprint
# Requires a selenium browser driver.


from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support.ui import Select
import time, getpass, datetime, csv, re, urllib, argparse, os.path, sys
from html.parser import HTMLParser
from pprint import pprint


### INIT ARGPARSE

# Parse cmdline args
parser = argparse.ArgumentParser(description=r"""Welcome to Morgan's  WIFI automation script.  Chances are you know what it does as you wrote it & you're probably the only user.

Instructions:

### First Run:
- Export device list from ASSET MANAGEMENT TOOL.
- Modify export from above step or create CSV with wifi devices you want to add to Wifi Webtool with the Model, AssetName and MACAddress in it with no heading.
- Run script with --AddDevices and let it do it's thing / export a CSV of added devices and download all new certs.
- Run script with --GenerateHTML argument and pass an input CSV file (exported by either of the above) and produce HTML/PHP output page.
- Copy all certs + HTML table to remote web server.  Make note of CSV name (or back it up) if you expect to use the CSV output again to generate new certs.
- Add certs on all devices.
- If a cert was successfully installed, remove device from the list of devices to add to the Wifi Webtool.

### Adding more devices:
- Modify above CSV or create new with wifi devices you want to add to the Wifi Webtool with the Model, AssetName and MACAddress in it with no heading.
- Run script with --AddDevices argument and let it do it's thing.  Any existing the Wifi Webtool entries that match devices being added will be removed (FROM WWT). Make note of output CSV name.
- Run script with --CombineCSVs argument and pass the file name exported by the above command as well as the csv output the second last time you ran the program.
It will output a new CSV that includes entries from both CSVs except it will omit any from the old CSV if they are also in the new CSV (based on MAC address).
- Optionally sort the outout CSV from --GenerateHTML in excel to desired arrangement and feed it back into the script.
- Run script with --GenerateHTML argument and pass an input CSV file (exported by either of the above) and produce HTML/PHP output.
- Copy all certs + HTML file to remote web server.  Make note of CSV name (or back it up) if you expect to use the CSV output again to generate new certs.
- Add certs on all devices.
- If a cert was successfully installed, remove device from the list of input CSV.

""", formatter_class=argparse.RawTextHelpFormatter)
  
parser.add_argument("--AddDevices", nargs=1, metavar=('DEVICE_CSV'), help="Provide CSV with list of devices you want to add to the Wifi Webtool.  Existing entries will be overwritten.", dest="AddDevices")
parser.add_argument("--CombineCSVs", nargs=2, metavar=('NEW_DEVICE_CSV', 'OLD_DEVICE_CSV'), help="Provide twos CSVs (new and old) output by AddDevices and they'll be combined. Any devices on both will be ignored from old list", dest="CombineCSVs")
parser.add_argument("--GenerateHTML", nargs=1, metavar=('DEVICE_CSV',), help="Generate HTML from previous CSV output from the above functions.", dest="GenerateHTML")

args = parser.parse_args()

### DEFINE SETTINGS 

wifiWebToolURL = "https://wifitool.internal.company.com"
wifiSSID = "CORP_SSID"

# File name of output files prefix
HTMLFileNamePrefix = "WIFI_Certs"
csvOutputNamePrefix = "WIFI_Certs"

# Debug / testing / hacky
devicesAddedCounter = 0 # used as a counter.
maxDevicesToAdd = 50 # Used for limiting loops during testing.
maxDevicesToDelete = 50 # Used for limiting loops during testing.

numberOfDaysUntilExpiration = int(360) # Cert expiry date

userUsername = "user_name" # Webtool username

# RegEx will match 1-99 if followed by " results". E.G. should match 26 out of "Displaying 1-25 of 26 results."  Used to know how many devices to iterate through.
regExPattern = r"\d{1,2}(?=\sresults)"

### These map the input CSV header titles (human readable) to the field/column number (but 0 indexed).  Really a DictReader should be used.
cModel = 0
cASSETNAME = 1
cMACAddress = 2

### Class used to log output to file as well as to screen.

class Tee(list):
	def write(self, obj):
		for s in self:
			s.write(obj)
	def flush(self):
		for s in self:
			s.flush()

### SUBROUTINE DEFINITIONS 

### Make sure input files exist.

def CheckInputFiles():
	if args.AddDevices != None:
		if not os.path.exists(args.AddDevices[0]):
			print("Exiting.  Can't find input file: " + args.AddDevices[0])
			exit()
	if args.GenerateHTML != None:
		if not os.path.exists(args.GenerateHTML[0]):
			print("Exiting.  Can't find input file: " + args.GenerateHTML[0])
			exit()
	if args.CombineCSVs != None:
		if not os.path.exists(args.CombineCSVs[0]):
			print("Exiting.  Can't find input file: " + args.CombineCSVs[0])
			exit()
		if not os.path.exists(args.CombineCSVs[1]):
			print("Exiting.  Can't find input file: " + args.CombineCSVs[1])
			exit()



### Init log file & set up sys.stdout Teeing

def InitLogFile():

	if args.AddDevices:
		logfileSuffix = "-AddDevices"
	elif args.GenerateHTML:
		logfileSuffix = "-GenerateHTML"
	elif args.CombineCSVs:
		logfileSuffix = "-CombineCSVs"
	else:
		logfileSuffix = ""

	#Logfile output
	if os.path.exists("RunLog" + dateNow.strftime("-%Y-%m-%d_%H-%M") + logfileSuffix + ".txt"):
		logFile = open("RunLog" + dateNow.strftime("-%Y-%m-%d_%H-%M") + logfileSuffix + ".txt","a")
	else:
		logFile = open("RunLog" + dateNow.strftime("-%Y-%m-%d_%H-%M") + logfileSuffix + ".txt","w+")
	
	print("Logging to file: RunLog" + dateNow.strftime("-%Y-%m-%d_%H-%M") + logfileSuffix + ".txt")

	streamList = [sys.stdout, logFile]
	global Tee
	sys.stdout = Tee(streamList)



### Init file objects used by GenerateHTML function.

def GenerateHTMLInitFiles():

	#Define globals
	global htmlOutputFile, csvInputFile, csvReaderNewDeviceList

	#php output
	htmlOutputFile = open(HTMLFileNamePrefix + dateNow.strftime("-%Y-%m-%d_%H-%M") + ".php","w+")

	# Open first input file
	inputCSVFields = ['Description', 'ExpireDate', 'MACAddress', 'CertificatePassword']
	csvInputFile = open(args.GenerateHTML[0], 'r', newline='')
	csvReaderNewDeviceList = csv.DictReader(csvInputFile, inputCSVFields)



def CombineCSVsInitFiles():
	#Define globals
	global csvInputFile, csvReaderNewDeviceList, csvOldInputFile, csvReaderOldDeviceList, csvOutputFile, csvWriter 
	
	csvFields = ['Description', 'ExpireDate', 'MACAddress', 'CertificatePassword']
	# Open new/first input file
	csvInputFile = open(args.CombineCSVs[0], 'r', newline='')
	csvReaderNewDeviceList = csv.DictReader(csvInputFile, csvFields)

	# Open old/second input file
	csvOldInputFile = open(args.CombineCSVs[1], 'r', newline='')
	csvReaderOldDeviceList = csv.DictReader(csvOldInputFile, csvFields)
	
	# Get new CSV ready for writing.
	csvOutputFile = open(csvOutputNamePrefix + dateNow.strftime("-%Y-%m-%d_%H-%M") + ".csv","w+", newline='')
	csvWriter = csv.DictWriter(csvOutputFile, csvFields, delimiter=',')



### Combine new device CSV and old device CSV 
def CombineCSVs():
	print("Going to combine " + args.CombineCSVs[0] + " and " + args.CombineCSVs[1] + ".  And output new CSV " + csvOutputNamePrefix + dateNow.strftime("-%Y-%m-%d_%H-%M") + ".csv")
	CombineCSVsInitFiles()

	csvInputFileContent = csvInputFile.read()
	csvOutputFile.write(csvInputFileContent + "\n")

	for oldRecordRow in csvReaderOldDeviceList:	
		csvInputFile.seek(0)
		for newRecordRow in csvReaderNewDeviceList:
			if oldRecordRow['MACAddress'] == newRecordRow['MACAddress']:
				print("VERBOSE: Skipping the following record from old input CSV as it exists in CSV: " + oldRecordRow['Description'])
				continue
			else:
				csvWriter.writerow({ \
					'Description': oldRecordRow['Description'], \
					'ExpireDate': oldRecordRow['ExpireDate'], \
					'MACAddress': oldRecordRow['MACAddress'], \
					'CertificatePassword': oldRecordRow['CertificatePassword'], \
				})
				print("VERBOSE: Added record from old input CSV: " + oldRecordRow['Description'])



### Generate HTML/PHP table from input CSV

def GenerateHTMLFromCSV():
	GenerateHTMLInitFiles()
	WriteHTMLTableHeader()
	
	print("Going to generate HTML doc from " + args.GenerateHTML[0] + " and output HTML file " + HTMLFileNamePrefix + dateNow.strftime("-%Y-%m-%d_%H-%M") + ".php")
	for row in csvReaderNewDeviceList:
		WriteHTMLDeviceRecord(row['Description'], row['ExpireDate'], row['MACAddress'], row['CertificatePassword'])
	
	print("Complete!")

	CloseHTMLFile()


### Write Device Record.  Tabs are not needed for rendering of output file but does make it's source code more readable.

def WriteHTMLDeviceRecord(Description, ExpireDate, MACAddress, CertificatePassword):
	htmlOutputFile.write('\t\t\t\t\t<tr align="center">\n')
	htmlOutputFile.write('\t\t\t\t\t\t<td>' + Description + '</td>\n')
	htmlOutputFile.write('\t\t\t\t\t\t<td>' + ExpireDate + '</td>\n')
	certificateLink = MACAddress.replace(":", "") + ".p12"
	htmlOutputFile.write('\t\t\t\t\t\t<td><A HREF="' + certificateLink + '">' + certificateLink + '</A></td>\n')
	htmlOutputFile.write('\t\t\t\t\t\t<td>' + CertificatePassword + '</td>\n')
	htmlOutputFile.write('\t\t\t\t\t</tr>\n')


### Write HTML Table Header

def WriteHTMLTableHeader():
	htmlOutputFile.write("<!-- Generated time: " + dateNow.strftime("%Y-%m-%d %H:%M:%S") + " -->\n")
	htmlOutputFile.write(r"""
<html>
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
		<title>Certs</title>
		<?php include \$_SERVER['DOCUMENT_ROOT']."/style.php"; ?>
	</head>
	<body>
		<?php include \$_SERVER['DOCUMENT_ROOT']."/navbar.php"; ?>
		<br>
  		<div class="container">
    		<div class="jumbotron">
        		<h1>WIFI Certs</h1>
        		<p><small>Certs for wifi</small></p>
    		</div>
    	<div class="panel panel-primary"><br>
""")
	# Write Table generated time
	htmlOutputFile.write("\t\t\t<h5 style=\"margin: 20px;\"><strong>Generated:</strong> " + dateNow.strftime("%Y-%m-%d %H:%M:%S") + "</h5>")

	# Write table header
	htmlOutputFile.write(r"""
			<table class="cert-table" width="100%">
				<tbody>
					<tr align="center">
						<td><b>DEVICE</b></td>
						<td><b>EXPIRY</b></td>
						<td><b>CERT LINK</b></td>						
						<td><b>PASSWORD</b></td>
					</tr>
""")



def CloseHTMLFile():
	# close table and then close files
	htmlOutputFile.write(r"""
				</tbody>
			</table>
		</div>
	</body>
</html>
""")
	htmlOutputFile.close()



def CloseCSVFile():
	csvOutputFile.close()


# Init global objects user by selenium scraping

def AddDevicesInitSelenium(): 
	#Create HTML parser object to be used to unescape HTML  
	global htmlParser
	htmlParser = HTMLParser()

	# Configure chrome to download into current folder & start selenium webdriver.
	currentDirectory = os.path.dirname(os.path.realpath(__file__))
	options = webdriver.ChromeOptions() 
	prefs = {'download.default_directory' : currentDirectory}
	options.add_experimental_option('prefs', prefs)

	global browser
	browser = webdriver.Chrome(chrome_options=options)


# Init objects used by file input and output for AddDevicesFunction
def AddDevicesInitFiles(): 
	# Open input & output files
	print("\nGoing to add devices to wifi webtool using " + args.AddDevices[0] + " as input. Output CSV will be: " + csvOutputNamePrefix + dateNow.strftime("-%Y-%m-%d_%H-%M") + ".csv")
	#declare as globals so objects can be used elsewhere
	global csvInputFile, csvWifiDevicesToAdd, htmlOutputFile, csvOutputFile, csvWriter
	
	#Open input CSV
	csvInputFile = open(args.AddDevices[0], 'r', newline='')
	csvWifiDevicesToAdd = csv.reader(csvInputFile, delimiter=',')

	#CSV output
	csvOutputFile = open(csvOutputNamePrefix + dateNow.strftime("-%Y-%m-%d_%H-%M") + ".csv","w+", newline='')
	csvWriter = csv.writer(csvOutputFile, delimiter=',')


# Actually add a device to wifi webtool.  Needs to be logged in and on WIFI webtool page.
def AddDeviceToWIFI(Model, ASSETNAME, MACAddress):
	owner = userUsername # Used to set device owner
	deviceDescription = ASSETNAME + " - " + Model # Used in the Wifi Webtool
	
	print("Add Device: \nDevice Description: " + deviceDescription + " MAC Address: " +  MACAddress + " Expiration: " + dateCertExpire.strftime("%Y-%m-%d") + " ...", end='')
	
	browser.execute_script("window.scrollTo(0, 0)")

	time.sleep(1)

	browser.find_element_by_xpath('//*[@id="refreshPage"]/img').click()
	
	time.sleep(3)
	
	# Click 'Create WIFI' Button
	browser.find_element_by_id('create_wifi_btn').click()
	time.sleep(2)
	
	#Select WIFI Network
	networkNameDropdown = Select(browser.find_element_by_id('Radius_wifiNetworkName'))
	networkNameDropdown.select_by_visible_text(wifiSSID)
	browser.find_element_by_id('Radius_wifiNetworkName').send_keys(Keys.RETURN)
	
	time.sleep(1)
	
	# Input MAC address
	radiusUsername = browser.find_element_by_id('Radius_username')
	radiusUsername.send_keys(str(MACAddress.upper()))
	
	#Input Device description
	radiusDescription = browser.find_element_by_id('Radius_description')
	radiusDescription.send_keys(deviceDescription)
	
	#Input owner
	radiusowner = browser.find_element_by_id('Radius_deviceOwner')
	radiusowner.send_keys(owner)
	
	#Input Expiration date of record
	radiusExpireDate = browser.find_element_by_id('Radius_expireDate')
	radiusExpireDate.send_keys(dateCertExpire.strftime("%Y-%m-%d"))
	radiusExpireDate.send_keys(Keys.RETURN)
	
	time.sleep(2)
	#Download the certificate.
	browser.find_element_by_xpath('//*[@id="download_cert_btn"]').click()
	
	time.sleep(1)
	
	#Get the Certificate password
	wifiCertificatePassword = browser.find_element_by_xpath('//*[@id="messageToUser"]').get_attribute('innerHTML')
	wifiCertificatePassword = wifiCertificatePassword.replace("Certificate password: ", "")
	
	# print("Unescaped password for certificate for above device is: " + wifiCertificatePassword + "\n")
	
	# Unescape any HTML entities
	wifiCertificatePassword = htmlParser.unescape(wifiCertificatePassword)
	
	#Click 'Create'
	browser.find_element_by_xpath('/html/body/div[4]/div[11]/div/button[1]').click()
	
	time.sleep(2)
	
	#Click 'Cancel' / close the window
	browser.find_element_by_xpath('/html/body/div[4]/div[11]/div/button[2]').click()
	time.sleep(1)
	
	# Write a record into the output CSV file
	csvWriter.writerow([deviceDescription, dateCertExpire.strftime("%Y-%m-%d"), str(MACAddress.upper()), wifiCertificatePassword])
	print("Success!")



### Login to the Wifi Webtool tool.  Requires selenium to be initialised:

def LoginToWWT():
	# login to WWT
	browser.get(wifiWebToolURL + '/site/login')

	print("\nEnter your WWT password for user: " + userUsername)
	userPassword = getpass.getpass()

	username = browser.find_element_by_id("LoginForm_username")
	password = browser.find_element_by_id("LoginForm_password")
	username.send_keys(userUsername)
	password.send_keys(userPassword)

	browser.find_element_by_name("yt0").click()

	time.sleep(2)

	browser.get(wifiWebToolURL + 'wifi/list')

	time.sleep(3)

	fullPageSource = browser.find_element_by_xpath('//*').get_attribute('outerHTML')

	if "User Management" in fullPageSource and "Logout" in fullPageSource:
		print("\nLogin Success!\n")
	else:
		print("\nError.  Page does not contain 'User Management' & 'logout' strings!\n")



### SCRIPT LOGIC of main function (AddDevice)

def AddDevices(): 
	#init selenium objects and files
	AddDevicesInitSelenium()
	AddDevicesInitFiles()

	global dateCertExpire
	dateCertExpire = dateNow + datetime.timedelta(days=numberOfDaysUntilExpiration)
	print("Any new certs will expire on: " + dateCertExpire.strftime("%Y-%m-%d"))

	LoginToWWT() # Logins to WWT tool

	print("Going to change number of records displayed to 100.")

	# table size selection (View 100 records and not just the default 25)

	tableSizeDropdown = Select(browser.find_element_by_id('pageSize'))
	tableSizeDropdown.select_by_visible_text("100")
	tableSizeDropdown = browser.find_element_by_id('pageSize').click()

	time.sleep(4)

	# Code to work out how many records to loop through to look for records that have to be removed
	tableDescriptionText = browser.find_element_by_xpath('//*[@id="wireless-devices-list"]/div[1]').get_attribute('innerHTML')
	numberOfRecordsTotalMatchObj = re.search(regExPattern, tableDescriptionText)
	numberOfRecordsTotal = int(numberOfRecordsTotalMatchObj.group(0))

	print("\nNumber of existing WIFI records on WWT: " + str(numberOfRecordsTotal))

	devicesAddedCounter = 0

	# Loop through each record looking for any records that match a MAC address in the input CSV of wifi certs to generate.  Then remove any that match from WWT.
	if maxDevicesToDelete != 0:
		print("Checking existing WWT records (based on MAC) that match any records in the input CSV of wifi certs we are to generate.  If it matches, the existing WWT record will be removed.\n")	
		for counter in range(1, numberOfRecordsTotal):
			browser.find_element_by_xpath('//*[@id="wireless-devices-list"]/table/tbody/tr[' + str(counter) + ']/td[5]/a[1]/img').click()
			time.sleep(2)
			
			MACAddr = browser.find_element_by_xpath('//*[@id="account_info"]/tbody/tr[1]/td[2]').text

			print("Checking MAC: " + MACAddr)
			
			# go to start of CSV that contains records to add to the WWT wifi
			csvInputFile.seek(0)
			
			# Loop through input CSV and look for duplicates by comparing MAC address
			for row in csvWifiDevicesToAdd:
				csvMACAddr = row[cMACAddress]
				csvMACAddr = csvMACAddr.upper()
				csvMACAddr = csvMACAddr.replace(":", "-")
				
				bRemoveRecord = False
				
				if csvMACAddr == MACAddr:
					bRemoveRecord = True
					print("This existing WWT record has matched a MAC address to a device being added.  Deleting existing record...", end="")
					break
			
			#Close browser pop up
			browser.find_element_by_xpath('/html/body/div[4]/div[11]/div/button').click()
			time.sleep(2)
			
			if bRemoveRecord == True:
				# Click the delete button on the existing record
				browser.find_element_by_xpath('//*[@id="wireless-devices-list"]/table/tbody/tr[' + str(counter) + ']/td[5]/a[3]/img').click()
				time.sleep(2)
				browser.find_element_by_xpath('/html/body/div[4]/div[11]/div/button[1]').click()
				time.sleep(2)
				browser.find_element_by_xpath('/html/body/div[4]/div[11]/div/button').click()
				time.sleep(1)
				
				print("Record removed. \n")
				
				# decrement loop variables so they still match the WWT table after deleting a record.
				numberOfRecordsTotal -= 1
				# print("loop counter value before dec:" + str(counter))
				counter -= 1
				# print("loop counter value after dec:" + str(counter))
				time.sleep(2)

			# Break if we have hit the set limit of devices (used for debug purposes)
			devicesAddedCounter += 1
			if devicesAddedCounter == maxDevicesToDelete:
				break

	print("\nFinished checking existing WWT records.  Now going to loop over list of devices to add to WWT (from input CSV) and create WWT records. \n")

	# Reset this in case we are undergoing testing and want to limit the number of devices to add
	devicesAddedCounter = 0
			
	### Loop over list of devices to add to WWT (from input CSV) and create WWT record	

	csvInputFile.seek(0) # Reset to start of CSV file

	for row in csvWifiDevicesToAdd:
		AddDeviceToWIFI(row[cModel], row[cASSETNAME], row[cMACAddress])
		devicesAddedCounter += 1
		if devicesAddedCounter == maxDevicesToAdd:
			break

	csvInputFile.close() #Close CSV input file
	csvOutputFile.close()

	print("Script / Add Devices complete! Output CSV is " + csvOutputNamePrefix + dateNow.strftime("-%Y-%m-%d_%H-%M") + ".csv" + " \nThank you come again!")



if __name__ == "__main__":
	CheckInputFiles()

	# Generate Dates (current and expiration)
	global dateNow
	dateNow = datetime.datetime.now()
	#Init log file and print current date/time
	InitLogFile()

	if args.AddDevices:
		AddDevices()
	elif args.GenerateHTML:
		GenerateHTMLFromCSV()
	elif args.CombineCSVs:
		CombineCSVs()
	else:
		parser.print_help()
	