# Running Commonwealth Cartography export scripts

### Prerequisites and assumptions
* A copy of Fallout 4 installed on your machine

## Familiar with FO4Edit?
If you know what you're doing with FO4Edit, you'll need to drop a FO4Edit installation onto the `\FO4Edit\` folder and then run the `_commonwealthCartography_RUNALL.pas` script. This will run all Commonwealth Cartography export scripts consecutively.<br/>
Once complete the folder `\FO4Edit\Output\` should be populated with 7 CSV files (including one informational CSV noting the debug cells for which extraction was skipped), and you can move on to the preprocessor.
<br/><br/>

## Not familiar with FO4Edit?

The Pascal scripts found in `\FO4Edit\Edit Scripts\` are built specifically to be used with the Fallout 4 Version of XEdit, also known as FO4Edit.<br/>

### Installing FO4Edit
FO4Edit is not distributed in this repository and you will need to grab it online.<br/>
The 'official' version of FO4Edit is [available on NexusMods](https://www.nexusmods.com/fallout4/mods/2737).<br/>

Once you have downloaded FO4Edit, you should simply be able to dump the contents of the zip into the `\FO4Edit\` folder. (You will note that the `Edit Scripts\` folder is already in place, populated with Commonwealth Cartography scripts).<br/>

### Launching FO4Edit
FO4Edit needs to be launched with the parameter `-D:"<Path_to_FO4_Data_folder>"`.<br/>
In the FO4Edit folder, you will find `SteamLaunch.bat`. If you have `FO4.exe` placed here and have Fallout 4 installed in the default Steam install location, you will be able to execute this batch file to fire up FO4Edit correctly. If your FO4Edit is under a different name, or your install location is different, please edit the script appropriately first.<br/>
Once in FO4Edit you will be prompted with which ESM to load. Select SeventySix.esm and hit OK. Wait for the background loader to finish.<br/>

### Running the Commonwealth Cartography export scripts
Expand SeventySix.esm in the tree on the left. Right click any element and select 'Apply Script...'.<br/>
Select `_commonwealthCartography_RUNALL` as your script to run.<br/>
This script runs all Commonwealth Cartography scripts consecutively and should take approximately 26 minutes.<br/>
Once completed you should see the folder `\FO4Edit\Output\` has been populated with 7 CSV files, including one informational CSV noting the debug cells for which extraction was skipped.<br/>
<br/>

### Next steps
Once you have successfully exported the initial data, you can now move on to preprocessing the data, please see [Preprocessor.md](Preprocessor.md)
