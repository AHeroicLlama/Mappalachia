@echo off

set releaseBuildFolder=bin\Release
set outputfile=Mappalachia.zip

echo Checking for Release build...
for %%f in ("Mappalachia.exe" "data\mappalachia.db") do if not exist "%releaseBuildFolder%\%%~f" (
	echo Mappalachia build not found in the %releaseBuildFolder% directory. Please rebuild from within VS under release configuration.
	PAUSE
	EXIT
)

echo Zipping release...
powershell Compress-Archive %releaseBuildFolder%\* %outputfile% -Force

echo done.
PAUSE
EXIT