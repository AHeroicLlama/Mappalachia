@echo off

title Mappalachia Database Builder

set sigcheckPath=sigcheck.exe
set steamPath="C:\Program Files (x86)\Steam\steamapps\common\Fallout76\Fallout76.exe"
set gameVersionFile=gameVersion.csv
set databaseFile=mappalachia.db
set summaryFile=summary.txt
set outputFolder=..\Mappalachia\data

IF NOT EXIST sqlite3.exe (
	echo sqlite3.exe was not found. Building the Mappalachia database requires sqlite3.exe to be placed in this folder.
	echo Please grab the sqlite-tools Windows binary from the official distribution at https://www.sqlite.org/download.html
	PAUSE
	EXIT
)

IF NOT EXIST "../Preprocessor/Output/Position_Data.csv" (
	echo Preprocessor output data was not found in the expected location. You must run the xEdit export scripts, then the Preprocessor before building the database.
	echo For more info please see development help guides.
	PAUSE
	EXIT
)

IF "%1"=="override" (
	echo Skipping automated game version checking.
) else (
	IF EXIST %steamPath% (
		IF EXIST %sigcheckPath% (
			echo Getting game version from exe via Sigcheck...
			echo version>%gameVersionFile%
			sigcheck.exe -nobanner -n %steamPath%>>%gameVersionFile%
		) else (
			echo Sigcheck was not found at path %sigcheckPath%. For automated version detection please download sigcheck.exe from sysinternals at https://docs.microsoft.com/en-gb/sysinternals/downloads/sigcheck
			echo Using manual version string from %gameVersionFile%...
		)
	) else (
		echo Steam Fallout76.exe was not found at %steamPath%. For automated version detection please ensure Fallout76.exe is installed/updated via Steam at the default location.
		echo Using manual version string from %gameVersionFile%...
	)
)

for /f %%l in (%gameVersionFile%) do set gameVersion=%%l

echo Is %gameVersion% the correct game version?
choice /c YN
if not %errorlevel%==1 (
	echo Please ensure that Fallout 76 is correctly intalled/updated via Steam, otherwise that the correct game version string is stored in %gameVersionFile%.
	echo If the game EXE itself has the wrong version number, call this script with the argument "override" and supply the correct version in %gameVersionFile%.
	PAUSE
	EXIT
)

echo Cleaning up old database files...
del %databaseFile% >nul 2>&1
del %databaseFile%-journal >nul 2>&1

echo Creating new empty tables...
sqlite3.exe %databaseFile% < sql/createTables.sql

echo Importing CSVs into new tables...
sqlite3.exe %databaseFile% < SQLiteBatchCSVImport.txt

echo Replacing previously escaped characters...
sqlite3.exe %databaseFile% < sql/replaceEscapedChars.sql

echo Building combined tables...
sqlite3.exe %databaseFile% < sql/buildCombinedTables.sql

echo Finding space ranges...
sqlite3.exe %databaseFile% < sql/getSpaceScales.sql

echo Trimming database...
sqlite3.exe %databaseFile% < sql/trimData.sql

echo Dropping once used tables...
sqlite3.exe %databaseFile% < sql/dropUnused.sql

echo Applying basic compression...
sqlite3.exe %databaseFile% < sql/compress.sql

echo Vacuum packing database...
sqlite3.exe %databaseFile% VACUUM

echo Building indices...
sqlite3.exe %databaseFile% < sql/createIndices.sql

echo Creating new summary with db file info...
echo ==Database name== > %summaryFile%
echo %databaseFile% >> %summaryFile%
echo ==SQLite version== >> %summaryFile%
sqlite3.exe --version >> %summaryFile%
echo ==Checksum== >> %summaryFile%
certutil -hashfile %databaseFile% MD5 | findstr /V ":" >> %summaryFile%
echo ==File Size== >> %summaryFile%
for %%f in (%databaseFile%) do echo %%~zf >> %summaryFile%

echo Listing tables and indices...
echo ==Tables== >> %summaryFile%
sqlite3.exe %databaseFile% .tables >> %summaryFile%

echo ==Indices== >> %summaryFile%
sqlite3.exe %databaseFile% .indices >> %summaryFile%

echo Populating main summary report...
sqlite3.exe %databaseFile% < sql/generateSummary.sql >> %summaryFile%

echo Moving database to Mappalachia program...
IF NOT EXIST %outputFolder% (
	mkdir %outputFolder%
)

move %databaseFile% %outputFolder% >nul

echo.
echo Finished!
PAUSE
