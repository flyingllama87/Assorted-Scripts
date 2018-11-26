:: Used to launch a powershell file with the same name as this batch file.
@ECHO OFF

PowerShell.exe -NoProfile -Command "& '%~dpn0.ps1'"


PAUSE

:: @ECHO OFF

