@echo off

title Commonwealth Cartography Release Packager

set publishFolder=publish
set outputFile=CommonwealthCartography.zip
set prefsFile=commonwealth_cartography.ini
set quickSaveFolder=QuickSaves

echo Checking for published build...
for %%f in ("CommonwealthCartography.exe" "data\commonwealth_cartography.db") do if not exist "%publishFolder%\%%~f" (
	echo Commonwealth Cartography build not found in the %publishFolder% directory. Please rebuild from within VS under release configuration.
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
