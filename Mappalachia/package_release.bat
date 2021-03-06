@echo off

set releaseBuildFolder=bin\Release
set outputFile=Mappalachia.zip
set prefsFile=mappalachia_prefs.ini

echo Checking for Release build...
for %%f in ("Mappalachia.exe" "data\mappalachia.db") do if not exist "%releaseBuildFolder%\%%~f" (
	echo Mappalachia build not found in the %releaseBuildFolder% directory. Please rebuild from within VS under release configuration.
	PAUSE
	EXIT
)

echo Removing preferences file prior to zipping...
del %releaseBuildFolder%\%prefsFile%

echo Zipping release...
powershell Compress-Archive %releaseBuildFolder%\* %outputFile% -Force

echo done.
PAUSE
EXIT
