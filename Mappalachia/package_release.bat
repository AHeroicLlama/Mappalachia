@echo off

title Mappalachia Release Packager

set publishFolder=publish
set outputFile=Mappalachia.zip
set prefsFile=mappalachia_prefs.ini

echo Checking for Release build...
for %%f in ("Mappalachia.exe" "data\mappalachia.db") do if not exist "%publishFolder%\%%~f" (
	echo Mappalachia build not found in the %publishFolder% directory. Please rebuild from within VS under release configuration.
	PAUSE
	EXIT
)

echo Removing preferences file prior to zipping...
del %publishFolder%\%prefsFile%

echo Zipping release...
powershell Compress-Archive %publishFolder%\* %outputFile% -Force

echo done.
PAUSE
EXIT
