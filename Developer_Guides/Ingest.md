# Ingesting preprocessed data and building the database

### Prerequisites and assumptions
* You have already run the [preprocessor](Preprocessor.md) to properly format the data
* (Optional) competency with SQLite SQL and Windows batch

## Mappalachia database
Mappalachia's database exists as an SQLite database file. SQLite allows us to have a locally hosted, offline database file. This means there are no hosting requirements for Mappalachia to function, and end users can use it offline.

## How is the database built?
Using SQLite, a single batch file creates the empty database structure, then instructs SQLite to populate the empty tables with the preprocessed data. After this further SQL generates several more tables based off existing data (For example, pre-computing min/max coords of cells as they cannot change), removes escaped characters, shrinks the database, indexes it, and generates a summary report.

## Game Version
The game version string is baked into the database. If you have Fallout 76 installed in the default Steam location, you may enable automatic game version detection by downloading [`sigcheck.exe`](https://docs.microsoft.com/en-gb/sysinternals/downloads/sigcheck) from Sysinternals and placing it in the database folder - if so, the game version string will be automatically gathered from `Fallout76.exe`.<br/>
Otherwise, before running `build_database.bat`, ensure the correct game version string is stored in `gameVersion.csv`.<br/>

## Building the database
Before we can build the database, we need a copy of the sqlite tools windows binary, called sqlite3.exe. This is distributed at the [SQLite downloads page](https://www.sqlite.org/download.html). Under 'Precompiled Binaries for Windows' find the sqlite-tools zip.<br/>
Once you have placed that exe in the `\Database\` folder, you can execute `build_database.bat` and after confirming the correct game version, the script will carry out all the steps required to build the database.<br/>
Once finished, the script will automatically move the db file to the data folder for the Mappalachia GUI at `\Mappalachia\data\`.

## Summary Report
You will notice a txt file in `\Database\` called `summary.txt`. This file is generated when the database is built and is intentionally source controlled.<br/>
Using git to visualize how the file changed, this report essentially acts as a canary for the reliability of data in the database, and allows us to quickly identify if anything has gone significantly wrong anywhere along the stage of exporting - as any wildly different values would indicate a problem.<br/>
Checking this report should be a key step in updating the database for a new game version. While we would expect the values to change, any large changes, or empty fields should immediately signify an error.

### Next steps
You should now have successfully extracted and assembled the Mappalachia database. You may optionally want to [extract the map marker icons](IconExtraction.md), or also optionally, [render the background images](BackgroundRendering.md). Otherwise you can move on to development of the actual [end-user GUI program, Mappalachia](GUI.md).
