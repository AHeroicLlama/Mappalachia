# Arranging and deploying a new release

### Prerequisites
* An installation of [7-Zip](https://www.7-zip.org/download.html)

### Note
Please be aware that every attempt has been made to ensure Mappalachia is resilient to any change which Bethesda may make, and the intention is that the tools described in these developer documents will continue to work without maintenance, however it remains entirely possible that Bethesda may without notice change their development approach, restructure the data, or otherwise add or remove game content which may cause the data to become inaccurate or invalid in some way.<br/>
It is recommended to stay alert to this when generating a new build. For bigger updates you may wish to briefly interact with the content in-game to ensure you understand what the correct data should look like.

## Building and releasing a new update - high level process
In order to package a Mappalachia release there are a few steps.
* Make sure the database has been newly compiled from the latest Fallout 76 version. ([Extraction with Edit Scripts](EditScripts.md) and [Preprocessing](preprocessor.md)).
* Verify using git that the database summary report (`BuildOutputs\Database_Summary.txt`) has not indicated any issues. Look for large or significant changes, or new or lost rows.
* Verify using git that the discarded cells report (`BuildOutputs\Discarded_Cells.csv`) does not indicate any genuine in-game cells have been incorrectly skipped.
* You should assess if any map marker icons may have changed or been added and if so, [perform the extraction](IconGeneration.md).
* If any cells or worldspaces have changed, you must [run the background renderer](BackgroundRendering.md) for those spaces. Review the space coordinate checksum section of the Database Summary to identify this. It is typically best to assume Appalachia has changed, and render it always.
* Run the spotlight patch/diff tool inside the background renderer to generate the spotlight tile patch. You should run this even if you didn't render anything new, since it will ensure the patch directory is cleared of old files.
* Use the validation functionality of the [Preprocessor](Preprocessor.md) to verify that image assets appear correct, particularly if any have been regenerated/extracted.
* Update the game version shield in `README.md`, and increment the Package Version in `Mappalachia.csproj`. Releases which only represent a game update should only increment the patch number.
* In Visual Studio, right click the Mappalachia Project and select 'Publish'. With the included `PublishProfile.pubxml` selected, press 'Publish'.
* Launch `Mappalachia\Package_Release.bat`. This requires a 7-Zip installation and assumes the default installation directory. It will use 7-Zip to create the `Mappalachia.zip`, `Patch_Spotlight.zip`, and additional 'spotlight' archives, placing them at `dist\`.
* `Mappalachia.zip` is the file which should now be distributed to end users. Additionally, `Patch_Spotlight.zip` should be made available as an optional download.
* All the `spotlight.7z.xxx` files should be attached to the [specific `spotlight` release on Github](https://github.com/AHeroicLlama/Mappalachia/releases/tag/spotlight), replacing all current assets there.

## Data troubleshooting & common errors to check
What follows is a list of changes you may need to make or check following a new game update.<br/>
It is useful to be familiar with `Library.HardCodings.cs`, as a lot of the "Bethesda jank" is accounted for here.

* Game Version: The `Meta` table in the database will capture the game version from the .exe. However, this typically *does not* align with the game version reported in-game (see Settings menu) or official patch notes. You should correct this in the preprocessor to match in-game, when prompted.
* New map markers: Map Markers often come out of XEdit with a different label than is actually in-game. You should double-check these in game, and correct them in `HardCodings.MarkerLabelCorrection`. It may also have the wrong icon (see `HardCodings.GetCorrectedMarkerIcon`). If a new icon image is released, refer to [IconGeneration](IconGeneration.md).
* New cells: A new cell will require being rendered, and likely aligned ("x/y correction") and z-cropped. See [BackgroundRendering](BackgroundRendering.md).
* Some new cells are copies of another (think daily ops). To ensure their x/y corrections remain the same, add these to `HardCodings.SisterSpaces`.
* If a new cell appears in the database summary with multiple NorthMarkers, identify the correct one and amend `HardCodings.NorthMarkerPreference`.
* As described above, ensure you check `BuildOutputs\Database_Summary.txt` and `BuildOutputs\Discarded_Cells.csv`. Discarded cells are detected via `HardCodings.DiscardCellsQuery`.
* A new scrap component, NPC, flux color, lock level, or signature (see XEdit) will likely require updates to enums, validation regexs, and other areas of code. You should therefore perform testing following this.
