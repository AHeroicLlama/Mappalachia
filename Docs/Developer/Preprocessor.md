# Preprocessing the exported data

### Prerequisites and assumptions
* You have already run the [export scripts](EditScripts.md) using FO76Edit
* An installation of Visual Studio
* A copy of `sqlite3.exe` in place (See '[How to use the Preprocessor](#how-to-use-the-preprocessor)')
* (Optional) competency with C# .NET and SQLite

## What is the Preprocessor?
The Mappalachia Preprocessor is a C# .NET CLI tool responsible for assembling and populating the database. It takes in the exported CSVs from XEdit and transforms and combines the data into its final form. It also performs validation against both the database and the existing image assets.<br/>

## How to use the Preprocessor
Before running, ensure that `sqlite3.exe` is placed into the `Utilities\` folder. This can be found at the [SQLite downloads page](https://www.sqlite.org/download.html), under precombiled binaries for Windows.

Find the Preprocessor project inside the main Mappalachia solution at `Mappalachia.sln`, then build and start the `Preprocessor` project.<br/>
If the database does not exist, it will automatically generate the database and perform data and image asset validation.<br/>
If the database already exists, you will have the option to re-build it, or just run validation options.<br>

The Preprocessor will ask you to confirm the current version of Fallout 76. It will fetch this from `Fallout76.exe` if you have it installed in the default Steam location. However, it is recommended to confirm this value in-game too, as the values are commonly different.<br/>
In the event they are different, the in-game value should be used.

The outputted database will be placed at `Assets\data\Mappalachia.db`.<br/>

Upon completion, the Preprocessor will generate the file `BuildOutputs\Database_Summary.txt`. This file is version controlled in git, and should be reviewed for changes whenever a new game update is released. Any unusual values or changes may indicate errors.<br/>
Similarly, it will generate `BuildOutputs\Discarded_Cells.csv`. This lists cells which were present in the ESM export, but were not kept in the database. This should be reviewed to ensure no cells were incorrectly discarded.

## Validation
The preprocessor also provides validation functionality, of both the database itself, and the existing image assets. These are automatically run when building the database, or may optionally be run alone.<br/>

If errors are found in either step they are outputted to the console at the end. The errors are also written to the file `BuildOutputs\Errors.txt`. These must be addressed before continuing.

### Next steps
Now the database has been assembled and validated, you can optionally [extract map icons](IconExtraction.md), [render map backgrounds](BackgroundRendering.md), or move to [development and deployment of the GUI itself](GUI.md).
