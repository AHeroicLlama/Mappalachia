# Mappalachia GUI

### Prerequisites and assumptions
* You have optionally [built and validated the database](Preprocessor.md), or, have a copy of `mappalachia.db` from a release
* You have optionally [extracted the map marker icons](IconGeneration.md), or, are confident they do not need updating
* You have optionally [rendered the background images](BackgroundRendering.md), or, have a copy of them all from a release
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
