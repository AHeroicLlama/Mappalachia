# Mappalachia GUI

### Prerequisites and assumptions
* You have optionally [built and validated the database](Preprocessor.md), or, have a copy of `mappalachia.db` from a release
* You have optionally [extracted the map marker icons](IconGeneration.md), or, are confident they do not need updating
* You have optionally [rendered the background images](BackgroundRendering.md), or, have a copy of them all from a release.
* An installation of [7-Zip](https://www.7-zip.org/download.html)
* An installation of Visual Studio
* Experience using Mappalachia as an end user
* Competency in C# .NET and SQLite SQL
* Reasonable Fallout 76 datamining knowledge

## Development of Mappalachia
Now that we are finished with the data mining part of the process, we can focus solely on the end-user product.<br/>
You will find the GUI project `Mappalachia` under the main solution, `Mappalachia.sln`<br/>

As this point, you should simply be able to 'start debugging' in Visual Studio to get it to run.
<br/>

## Debugging without building assets
Building the database and generating image assets is a multi-step process and you don't *need* to build them to debug the Mappalachia GUI, you just need working copies of them.<br/>

If you want to debug Mappalachia and skip out on the datamining and asset generations steps, you need to grab a copy of the pre-assembled assets from a release.<br/>
To use these in your debugging, copy the `img\` and `data\` folders from the root of a release, and place them in the `Assets\` folder at the root of the repository, then rebuild the project. A post-build event will copy them to the relevant location(s) in `bin\`, therefore allowing you to debug.
<br/>

## Packaging a release.
In order to package a Mappalachia release there are a few steps.
* Make sure the database has been newly compiled from the latest Fallout 76 version. ([Extraction with Edit Scripts](EditScripts.md) and [Preprocessing](preprocessor.md)).
* Verify using git that the database summary report (`BuildOutputs\Database_Summary.txt`) has not indicated any issues. Particularly look for large changes in averaged values, or any changes in cell table info.
* Verify using git that the discarded cells report (`BuildOutputs\Discarded_Cells.csv`) does not indicate any in-game cells have been incorrectly skipped.
* You should assess if any map marker icons may have changed or been added and if so, [run the extraction](IconGeneration.md).
* If any cells or worldspaces have changed, you must [run the background renderer](BackgroundRendering.md) for those spaces. Review the space coordinate checksum section of the Database Summary to identify this.
* Use the validation functionality of the [Preprocessor](Preprocessor.md) to verify that image assets appear correct, particularly if any have been regenerated/extracted.
* In Visual Studio, right click the Mappalachia Project and select 'Publish'. With the included `PublishProfile.pubxml` selected, press 'Publish'.
* Launch `Mappalachia\Package_Release.bat`. This requires a 7-Zip installation and assumes the default installation directory. It will use 7-Zip to create the `Mappalachia.zip`, `Patch_Spotlight.zip`, and additional 'spotlight' archives, placing them at `dist\`.
* `Mappalachia.zip` is the file which should now be distributed to end users. Additionally, `Patch_Spotlight.zip` and all the `spotlight.7z.xxx` files should be made available as optional downloads.
