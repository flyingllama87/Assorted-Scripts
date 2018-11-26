#
# 
#

#!/usr/bin/env python3
import os, hashlib, csv, datetime, argparse, colorama, sys, win32api, win32con, win32security

# Init ArgParse
parser = argparse.ArgumentParser()
parser.add_argument("--ScanArtFiles", help="Recursively scan all folders listed in file_with_all_art_folders var, generating a CSV file (filename_list_of_all_art_files var) with a list of files, sizes of folders and an MD5 checksum of them all", action='store_true', dest="ScanArtFiles")
parser.add_argument("--ScanSVNArtFiles", help="Recursively scan folder structure specified by path_with_SVN_art_files var, generating a CSV file (filename_list_of_all_svn_art_files var) with a list of all files, sizes of folders and an MD5 checksum of them all", action='store_true', dest="ScanSVNArtFiles")
parser.add_argument("--CompareMD5Sums", help="Compare MD5 Checksums of art share files to checksums of SVN files to find what's missing from SVN", nargs=2, metavar=('ART_SHARE_FILES_CSV', 'SVN_FILES_CSV'), dest="CompareMD5Sums")
parser.add_argument("--RemovePSDExports", help="Remove PNGs, JPGs, PDFs, TIFs, and TGAs where a PSD also exists in the same folder with the same name", nargs=1, metavar=('CSV_LIST_OF_FILES'), dest="RemovePSDExports")
parser.add_argument("--FillSVNInfo", help="Fill in the data for SVN Comment which is path of file, file owner, modified date", nargs=1, metavar=('CSV_LIST_OF_FILES'), dest="FillSVNInfo")

args = parser.parse_args()


# Script Settings
file_with_all_art_folders = "C:\\ART_FILES_SCRIPT\\LIST_OF_ART_FOLDERS.TXT"
filename_list_of_all_art_files = "ART_SHARE_FILES.CSV"

path_with_SVN_art_files = "D:\\Develop\\art"
filename_list_of_svn_art_files = "SVN_ART_FILES.CSV"

output_folder_name = datetime.datetime.now().__str__()[:19].replace(' ', '_').replace(':','-')
output_folder_path = os.path.join(os.getcwd(), output_folder_name)

colorama.init()

# Calculate MD5 hash for file data.
def file_md5sum(file_name):
    hash_md5 = hashlib.md5()
    with open(file_name, "rb") as f:
        for chunk in iter(lambda: f.read(4096), b""):
            hash_md5.update(chunk)
    return hash_md5.hexdigest()

def scan_art_files():
	filepath_list_of_all_art_files = os.path.join(output_folder_path, filename_list_of_all_art_files)

	print("Writing to file: " + filepath_list_of_all_art_files)

	with open(filepath_list_of_all_art_files, 'w') as output_file:
		# csv.writer(output_file, delimiter=',', quotechar='"', quoting=csv.QUOTE_MINIMAL)
		output_file.write("FULLPATH,FILENAME,FILESIZE,MD5SUM,CHECKEDMD5,CHECKEDFUZZY\n")

	with open(file_with_all_art_folders, 'r') as file:
		list_of_all_art_folders = file.read().splitlines()

	for art_folder in list_of_all_art_folders:
		print(colorama.Fore.RED + "\nScanning path: ", art_folder, colorama.Style.RESET_ALL)
		with open(filepath_list_of_all_art_files, 'a', newline='') as output_file:
			csv_output_file = csv.writer(output_file, delimiter=',', quotechar='"', quoting=csv.QUOTE_MINIMAL)
			for rootpath, dirs, files in os.walk(art_folder):
				if '.svn' in dirs:
					dirs.remove('.svn') # don't visit SVN folders			
				for file in files:
					file_full_path = os.path.join(rootpath, file)
					print("Scanning file: " + file_full_path)
					md5sum = file_md5sum(file_full_path)
					print("finished generating md5 sum: " + md5sum)
					csvrow = [file_full_path,
						file,
						os.path.getsize(file_full_path),
						md5sum]
					print(csvrow)
					csv_output_file.writerow(csvrow)

# Recursively scan folder structure specified by path_with_SVN_art_files var, generating a CSV file (filename_list_of_all_svn_art_files var) with a list of all files, sizes of folders and an MD5 checksum of them all
def scan_svn_art_files():
	filepath_list_of_svn_art_files = os.path.join(output_folder_path, filename_list_of_svn_art_files)

	print("Writing to file: " + filepath_list_of_svn_art_files)

	with open(filepath_list_of_svn_art_files, 'w') as output_file:
		output_file.write("FULLPATH,FILENAME,FILESIZE,MD5SUM\n")

	print(colorama.Fore.RED, "\nScanning path: ", path_with_SVN_art_files, colorama.Style.RESET_ALL)
	with open(filepath_list_of_svn_art_files, 'a', newline='') as output_file:
		csv_output_file = csv.writer(output_file, delimiter=',', quotechar='"', quoting=csv.QUOTE_MINIMAL)
		for rootpath, dirs, files in os.walk(path_with_SVN_art_files):
			if '.svn' in dirs:
				dirs.remove('.svn') # don't visit SVN folders			
			for file in files:
				file_full_path = os.path.join(rootpath, file)
				print("Scanning file: " + file_full_path)
				md5sum = file_md5sum(file_full_path)
				print("finished generating md5 sum: " + md5sum)
				csvrow = [file_full_path,
					file,
					os.path.getsize(file_full_path),
					md5sum]
				print(csvrow)
				csv_output_file.writerow(csvrow)

# Compare MD5 Checksums of art share files to checksums of SVN files to find what's missing from SVN
def compare_md5_sums():
	if os.path.exists(args.CompareMD5Sums[0]) is False:
		print("Your first file given for the compare MD5 checksum function (CSV of all files) does not exist!")
		sys.exit()
	if os.path.exists(args.CompareMD5Sums[1]) is False:
		print("Your second file for the compare MD5 checksum function (CSV of SVN files) does not exist!")
		sys.exit()
	os.makedirs(output_folder_path)
	
	filename_csv_of_files_with_match = "ART_FILES_MD5_MATCH.CSV"
	filename_csv_of_files_with_no_match = "ART_FILES_NO_MD5_MATCH.CSV"
	filepath_csv_of_files_with_match = os.path.join(output_folder_path, filename_csv_of_files_with_match)
	filepath_csv_of_files_with_no_match = os.path.join(output_folder_path, filename_csv_of_files_with_no_match)
	filepath_new_list_of_svn_art_files = os.path.join(output_folder_path, "NEW_" + filename_list_of_svn_art_files)

	# Write column headers in CSV

	with open(filepath_csv_of_files_with_match, 'w', newline='') as output_file_matches:
		output_file_matches.write("MD5SUM,FILESIZE,FILENAMEA,FULLPATHA,FILENAMEB,FULLPATHB\n")

	with open(filepath_csv_of_files_with_no_match, 'w', newline='') as output_file_no_matches:
		output_file_no_matches.write("MD5SUM,FILESIZE,FILENAME,FULLPATH,FILEOWNER,SVNCOMMENT,SVNCOMMITED\n")

	with open(filepath_new_list_of_svn_art_files, 'w', newline='') as output_file:
		output_file.write("FULLPATH,FILENAME,FILESIZE,MD5SUM,CHECKEDMD5,CHECKEDFUZZY\n")

	with open(args.CompareMD5Sums[0], 'r') as infile_art_share_files, \
		 open(filepath_csv_of_files_with_match, 'a', newline='') as outfile_matches, \
		 open(filepath_csv_of_files_with_no_match, 'a', newline='') as outfile_no_matches, \
		 open(filepath_new_list_of_svn_art_files, 'a', newline='') as outfile_art_share_files:

			csv_matches_fields = ['MD5SUM', 'FILESIZE' , 'FILENAMEA' , 'FULLPATHA' , 'FILENAMEB' , 'FULLPATHB']
			csv_no_matches_fields = ['MD5SUM', 'FILESIZE', 'FILENAME', 'FULLPATH', 'FILEOWNER', 'SVNCOMMENT', 'SVNCOMMITED']
			csv_outfile_art_share_files_fields = ['FULLPATH', 'FILENAME', 'FILESIZE', 'MD5SUM', 'CHECKEDMD5', 'CHECKEDFUZZY']

			csv_art_share_files = csv.DictReader(infile_art_share_files)
			csv_matches = csv.DictWriter(outfile_matches, csv_matches_fields)
			csv_no_matches = csv.DictWriter(outfile_no_matches, csv_no_matches_fields)
			csv_outfile_art_share_files = csv.DictWriter(outfile_art_share_files, csv_outfile_art_share_files_fields)

			for art_share_file_row in csv_art_share_files:
				bMatchedFile = False
				with open(args.CompareMD5Sums[1], 'r') as infile_svn_art_files:
					csv_svn_art_files = csv.DictReader(infile_svn_art_files)
					for svn_art_file_row in csv_svn_art_files:
						if bMatchedFile: 
							continue # If we've previously found a match for this file, skip it.
						# If we've found a matching file based on MD5, write an entry into the CSV of matched files.
						if art_share_file_row['MD5SUM'] == svn_art_file_row['MD5SUM']:
							csv_matches.writerow({ \
								'MD5SUM': art_share_file_row['MD5SUM'], \
								'FILESIZE': art_share_file_row['FILESIZE'], \
								'FILENAMEA': art_share_file_row['FILENAME'], \
								'FULLPATHA': art_share_file_row['FULLPATH'], \
								'FILENAMEB': svn_art_file_row['FILENAME'], \
								'FULLPATHB': svn_art_file_row['FULLPATH']
							})
							bMatchedFile = True
					# If we haven't found a match, write an entry into the CSV of non-matched files.
					if not bMatchedFile:
						csv_no_matches.writerow({ \
							'MD5SUM': art_share_file_row['MD5SUM'], \
							'FILESIZE': art_share_file_row['FILESIZE'], \
							'FILENAME': art_share_file_row['FILENAME'], \
							'FULLPATH': art_share_file_row['FULLPATH'], \
						})
					# regardless of us finding a match, update the art files CSV with a list of checked files.
					csv_outfile_art_share_files.writerow({ \
							'MD5SUM': art_share_file_row['MD5SUM'], \
							'FILESIZE': art_share_file_row['FILESIZE'], \
							'FILENAME': art_share_file_row['FILENAME'], \
							'FULLPATH': art_share_file_row['FULLPATH'], \
							'CHECKEDMD5': "YES" \
					})

# Remove PNGs, JPGs, PDFs, TIFs, and TGAs where a PSD also exists in the same folder with the same name		
def remove_PSD_exports():
	if os.path.exists(args.RemovePSDExports[0]) is False:
		print("The CSV file given for the function does not exist!")
		sys.exit()
	os.makedirs(output_folder_path)
	csv_fields = ['MD5SUM', 'FILESIZE', 'FILENAME', 'FULLPATH', 'DESTINATION', 'ARTDRIVEPATH', 'SVNCOMMENT', 'SVNCOMMITED']
	export_file_formats = ['jpg', 'tif', 'pdf', 'png', 'tga']
	filepath_output_file = os.path.join(output_folder_path, "FILES_TO_COMMIT.CSV")
	input_filepath = args.RemovePSDExports[0]

	# Write column headers in CSV

	with open(filepath_output_file, 'w', newline='') as output_file:
		output_file.write("MD5SUM,FILESIZE,FILENAME,FULLPATH,DESTINATION,ARTDRIVEPATH,SVNCOMMENT,SVNCOMMITED\n")

	with open(input_filepath, 'r') as input_file:
		input_csv = csv.DictReader(input_file, csv_fields)
		
		with open(filepath_output_file, 'a', newline='') as output_file:
			output_csv = csv.DictWriter(output_file, csv_fields)
			for index, image_row in enumerate(input_csv):
				bDoNotCopy = False																	# Used to choose whether we copy this entry into the output file or not.
				if image_row['FULLPATH'][-3:].lower() in export_file_formats: 						# Check if the file entry we are checking is the type of a typical exported file format.
					image_file_no_ext = image_row['FULLPATH'][:-4].lower() 							# Get the name of the file without the extension. 
					with open(input_filepath, 'r') as input_file_2: 								# Open the same input file a second time to scan for other files with the same filename
						input_csv_2 = csv.DictReader(input_file_2, csv_fields)				
						for psd_row in input_csv_2:													# Loop over each row looking for PSDs files with the same name as the file we are scanning
							if psd_row['FULLPATH'].lower() == image_file_no_ext.lower() + ".psd":
								bDoNotCopy = True
								print("Found the file", image_row['FULLPATH'], "which is probably an export of", psd_row['FULLPATH'])
				if not bDoNotCopy:
					# If this isn't an exported file, write an entry into the output CSV file
					output_csv.writerow({ \
							'MD5SUM': image_row['MD5SUM'], \
							'FILESIZE': image_row['FILESIZE'], \
							'FILENAME': image_row['FILENAME'], \
							'FULLPATH': image_row['FULLPATH']
					})
				if (index % 100) == 0:
					print(colorama.Fore.RED, "Currently Processing file:", image_row['FULLPATH'], "Index: ", index, colorama.Style.RESET_ALL)

# Fill in the data for SVN Comment which is path of file, file owner, modified date
def fill_svn_info():
	if os.path.exists(args.FillSVNInfo[0]) is False:
		print("The CSV file given for the function does not exist!")
		sys.exit()
	os.makedirs(output_folder_path)
	# basepath_to_replace = 'Y:\\ART_FILES\\'
	csv_fields = ['MD5SUM', 'FILESIZE', 'FILENAME', 'FULLPATH', 'DESTINATION', 'ARTDRIVEPATH', 'SVNCOMMENT', 'SVNCOMMITED']
	filepath_output_file = os.path.join(output_folder_path, "FILES_TO_COMMIT.CSV")
	input_filepath = args.FillSVNInfo[0]

	# Write column headers in CSV
	with open(filepath_output_file, 'w', newline='') as output_file:
		output_file.write("MD5SUM,FILESIZE,FILENAME,FULLPATH,DESTINATION,ARTDRIVEPATH,SVNCOMMENT,SVNCOMMITED\n")

	with open(input_filepath, 'r') as input_file:
		input_csv = csv.DictReader(input_file, csv_fields)
		with open(filepath_output_file, 'a', newline='') as output_file:
			output_csv = csv.DictWriter(output_file, csv_fields)
			for index, row in enumerate(input_csv):
				if index == 0: # Skip first line with column headers
					continue
				# Get art file from artists share instead of N:\ Drive (fast NAS)
				art_file = row['FULLPATH']
				# art_file = row['FULLPATH'].replace(basepath_to_replace, row['ARTDRIVEPATH'])
				if not os.path.exists(art_file):
					print("Quitting.  Can't find the file: ", art_file)
					assert(os.path.exists(art_file))	# Assert if file does not exist as something has gone horribly wrong.
				# Get last modified time of the file.
				art_file_modified_time = datetime.datetime.fromtimestamp(os.path.getmtime(art_file)).strftime('%Y-%m-%d %H:%M')
				# Get the username owner/creator of the file.
				art_file_security_descriptor = win32security.GetFileSecurity (art_file, win32security.OWNER_SECURITY_INFORMATION)
				art_file_owner_sid = art_file_security_descriptor.GetSecurityDescriptorOwner ()
				art_file_owner_name, domain, type = win32security.LookupAccountSid(None, art_file_owner_sid)
				# Generate the SVN comment for the file & print it.				
				svn_comment = "- Add art asset \"" + row['FILENAME'] + "\" created by user " + art_file_owner_name + " and last modified " + art_file_modified_time
				print(svn_comment)
				# Output the CSV row
				output_csv.writerow({ \
					'MD5SUM': row['MD5SUM'], \
					'FILESIZE': row['FILESIZE'], \
					'FILENAME': row['FILENAME'], \
					'FULLPATH': art_file, \
					'DESTINATION': row['DESTINATION'], \
					'ARTDRIVEPATH': row['ARTDRIVEPATH'], \
					'SVNCOMMENT': svn_comment
					})
				if (index % 100) == 0:
					print(colorama.Fore.RED, "Currently Processing index ", index, " out of all files", colorama.Style.RESET_ALL)


if __name__ == "__main__":
	if len(sys.argv) == 1:
		parser.print_help()
	if (args.ScanArtFiles or args.ScanSVNArtFiles):
		os.makedirs(output_folder_path)
	if args.ScanArtFiles:
		scan_art_files()
	if args.ScanSVNArtFiles:
		scan_svn_art_files()
	if args.CompareMD5Sums:
		compare_md5_sums()
	if (args.RemovePSDExports):
		remove_PSD_exports()
	if args.FillSVNInfo:
		fill_svn_info()
	