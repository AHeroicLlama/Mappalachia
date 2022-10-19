@echo off

title Mappalachia Release Packager

set publishFolder=publish
set outputFile=Mappalachia.zip
set prefsFile=mappalachia_prefs.ini
set quickSaveFolder=QuickSaves

echo Checking for published build...
for %%f in ("Mappalachia.exe" "data\mappalachia.db") do if not exist "%publishFolder%\%%~f" (
	echo Mappalachia build not found in the %publishFolder% directory. Please rebuild from within VS under release configuration.
	PAUSE
	EXIT
)

echo Removing preferences file and QuickSaves folder prior to zipping...
del %publishFolder%\%prefsFile% > nul 2>&1
rmdir /s %publishFolder%\%quickSaveFolder% /q > nul 2>&1

echo Zipping release...
powershell Compress-Archive %publishFolder%\* %outputFile% -Force

echo done.
PAUSE
EXIT
