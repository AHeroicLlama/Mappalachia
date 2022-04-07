# Mappalachia GUI

### Prerequisites and assumptions
* You have already [built the database](Ingest.md) OR have a copy of `mappalachia.db` from a release
* An installation of Visual Studio
* Experience using Mappalachia as an end user
* Competency in C# .Net and SQLite SQL
* Reasonable Fallout 76 datamining knowledge

## Development of Mappalachia
Now that we are finished with the 'data mining' part of the process, we can focus solely on the end-user product.<br/>
You will find the entire project under `\Mappalachia\Mappalachia.sln`<br/>
Mappalachia uses the .Net Framework V4.8, and also relies heavily on Microsoft.Data.Sqlite package plus others to provide connectivity to the database.<br/>

As this point, you should simply be able to 'start debugging' in Visual Studio to get it to run.
<br/>

## Debugging without building the database
Building the database is a multi-step process and you don't *need* to build it to debug the Mappalachia GUI, you just need a working copy of it.<br/>

If you want to debug Mappalachia and skip out on the datamining steps, you need to grab a copy of the pre-assembled `mappalachia.db` from a release. This can be found in `\data\mappalachia.db`.<br/>
To use this copy in your debugging, you should recreate the `\Mappalachia\data\` folder (alongside the `img\` and `font\` folders) and place `mappalachia.db` inside. Then rebuild the solution, and a post-build event will copy this to the relevant location(s) in `bin\`, therefore allowing you to debug.
<br/>

## Packaging a release.
In order to package a Mappalachia release there are a few steps.
* Make sure the database has been newly compiled from the latest Fallout 76 version.
* Verify using git that the database summary report (`Database\summary.txt`) has not indicated any issues.
* Verify using git that the skipped cells report (`FO76Edit\Output\SkippedCells.csv`) does not indicate any in-game cells have been incorrectly skipped.
* Make sure to 'rebuild solution' to build the Mappalachia program in `Release` configuration.
* Launch `Mappalachia\package_release.bat`. This batch script will essentially just check for a release build and then zip up the `bin\Release\` folder and drop it in the `\Mapplachia\` folder besides the batch script.
* `Mappalachia.zip` is the file which should now be distributed to end users.
