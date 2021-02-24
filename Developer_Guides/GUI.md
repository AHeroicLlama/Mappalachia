# Mappalachia GUI

### Prerequisites and assumptions
* You have already [built the database](Ingest.md) OR have a copy of `mappalachia.db` from a release
* An installation of Visual Studio 2019
* Experience using Mappalachia as an end user
* Competency in C# .Net Framework and SQLite SQL
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

## Notes
Failing full-scale code documentation, I will list the following key points to be aware of/understand;<br/>

* There are three folders - Class, Form, and SQL.
* SQL holds all the SQL scripts (stored as project resources) which are executed during runtime to access the database for varying reasons.
* Form holds the 4 forms required. FormMaster is clearly the largest.
* Class holds a mixture of static and non-static classes
* There is an important distinction between `MapItem` and `MapDataPoint`. When the user searches for items, they are a returned a list of all matches, but they are *not* returned a list of all *instances*. This is because the search results and legend lists represent a list of `MapItem`, which itself represents a game object. On the other hand, when this item is plotted on the map, we then need to convert a `MapItem` to a collection of `MapDataPoint`. A `MapDataPoint` represents a *unique* single instance of a game object.
* It is helpful to understand the three types of search item which Mappalachia distinguishes. These are defined in `MapItem.Type`.
* The large majority of work done by the program is shared between `FormMaster` and `Map`. As you can imagine, `FormMaster` manages all the main UI controls, but it also holds the current search results and legends items. `Map` On the other hand does all the drawing of the map.
* Almost all database queries ultimately come from `FormMaster`. These queries go to `Queries` which directly executes the SQL. Often `DataHelper` is used in-between to translate the data into something more user-friendly, or otherwise handle the query.
* User-customisable settings and some defaults are stored in the three classes prefixed `Settings`. Other non-editable settings lie in their respective classes, such as at the top of `Map`.
* There are post-build events configured to copy `font\`, `data\`, and `img\` to the build directory. If Mappalachia is complaining it can't find those files, you may just need to Rebuild.

## Packaging a release.
In order to package a Mappalachia release there are a few steps.
* Make sure the database has been newly compiled from the latest Fallout 76 version.
* Verify the database summary report has not indicated any issues.
* Make sure to 'rebuild solution' to build the Mappalachia program in `Release` configuration.
* Launch `Mappalachia\package_release.bat`. This batch script will essentially just check for a release build and then zip up the `bin\Release\` folder and drop it in the `\Mapplachia\` folder besides the batch script.
* `Mappalachia.zip` is the file which should now be distributed to end users.
