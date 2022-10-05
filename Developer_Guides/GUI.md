# Mappalachia GUI

### Prerequisites and assumptions
* You have optionally [built the database](Ingest.md), or, have a copy of `mappalachia.db` from a release
* You have optionally [extracted the map marker icons](IconExtraction.md), or, are confident they do not need updating
* You have optionally [rendered the background images](BackgroundRendering.md), or, have a copy of them all from a release.
* You have optionally [validated all image assets are present](ImageAssetValidation.md)
* An installation of Visual Studio
* Experience using Mappalachia as an end user
* Competency in C# .NET and SQLite SQL
* Reasonable Fallout 76 datamining knowledge

## Development of Mappalachia
Now that we are finished with the 'data mining' part of the process, we can focus solely on the end-user product.<br/>
You will find the GUI project `Mappalachia` under the main solution, `Mappalachia.sln`<br/>
Mappalachia uses .NET 6, and also relies heavily on Microsoft.Data.Sqlite package plus others to provide connectivity to the database.<br/>

As this point, you should simply be able to 'start debugging' in Visual Studio to get it to run.
<br/>

## Debugging without building the database
Building the database is a multi-step process and you don't *need* to build it to debug the Mappalachia GUI, you just need a working copy of it.<br/>

If you want to debug Mappalachia and skip out on the datamining steps, you need to grab a copy of the pre-assembled `mappalachia.db` from a release. This can be found in `\data\mappalachia.db`.<br/>
To use this copy in your debugging, you should recreate the `\Mappalachia\data\` folder (alongside the `img\` and `font\` folders) and place `mappalachia.db` inside. Then rebuild the solution, and a post-build event will copy this to the relevant location(s) in `bin\`, therefore allowing you to debug.
<br/>

## Packaging a release.
In order to package a Mappalachia release there are a few steps.
* Make sure the database has been newly compiled from the latest Fallout 76 version. (Including all prior steps - [Extraction with Edit Scripts](EditScripts.md) and [Preprocessing](preprocessor.md)).
* Verify using git that the database summary report (`Database\summary.txt`) has not indicated any issues. Particularly look for large changes in averaged values, or any changes in cell table info.
* Verify using git that the skipped cells report (`FO76Edit\Output\SkippedCells.csv`) does not indicate any in-game cells have been incorrectly skipped.
* You should assess if any map marker icons may have changed or been added and if so, [run the extraction](IconExtraction.md).
* You should assess if any cells or worldspaces have been added or have changed in a way significant enough to outdate their rendered background image. If so, [run the background renderer](BackgroundRendering.md) for those spaces.
* Use the [Image Asset Validator](ImageAssetValidation.md) to verify that at least all required image assets are present.
* In Visual Studio, right click the Mappalachia Project and select 'Publish'. With the included `PublishProfile.pubxml` selected, press 'Publish'.
* Launch `Mappalachia\package_release.bat`. This batch script will just check for a published build, remove any generated preferences file and then zip up the `publish` folder, leaving the zip in the folder besides the batch script.
* `Mappalachia.zip` is the file which should now be distributed to end users.
