# Running Mappalachia export scripts

### Prerequisites and assumptions
* A copy of Fallout 76 installed on your machine

## Familiar with FO76Edit?
If you know what you're doing with FO76Edit, you'll need to drop a FO76Edit installation onto the `\FO76Edit\` folder and then run the `_mappalachia_RUNALL.pas` script. This will run all Mappalachia export scripts consecutively.<br/>
Once complete the folder `\FO76Edit\Output\` should be populated with 7 CSV files (including one informational CSV noting the debug cells for which extraction was skipped), and you can move on to the preprocessor.
<br/><br/>

## Not familiar with FO76Edit?

The Pascal scripts found in `\FO76Edit\Edit Scripts\` are built specifically to be used with the Fallout 76 Version of XEdit, also known as FO76Edit.<br/>

### Installing FO76Edit
FO76Edit is not distributed in this repository and you will need to grab it online.<br/>
The 'official' version of FO76Edit is [available on NexusMods](https://www.nexusmods.com/fallout76/mods/30).<br/>
However for a more regularly updated version, user Eckserah has spent significant time maintaining an up to date version mostly in line with Bethesda's patches. This version is [available via NukaCrypt](https://nukacrypt.com/ecksedit/latest).<br/>

Once you have downloaded FO76Edit, you should simply be able to dump the contents of the zip into the `\FO76Edit\` folder. (You will note that the `Edit Scripts\` folder is already in place, populated with Mappalachia scripts).<br/>

### Launching FO76Edit
FO76Edit needs to be launched with the parameter `-D:"<Path_to_FO76_Data_folder>"`.<br/>
In the FO76Edit folder, you will find `SteamLaunch.bat`. If you have `FO76Edit64.exe` placed here and have Fallout 76 installed in the default Steam install location, you will be able to execute this batch file to fire up FO76Edit correctly. If your FO76Edit is under a different name, or your install location is different, please edit the script appropriately first.<br/>
Once in FO76Edit you will be prompted with which ESM to load. Select SeventySix.esm and hit OK. Wait for the background loader to finish.<br/>

### Running the Mappalachia export scripts
Expand SeventySix.esm in the tree on the left. Right click any element and select 'Apply Script...'.<br/>
Select `_mappalachia_RUNALL` as your script to run.<br/>
This script runs all Mappalachia scripts consecutively and should take approximately 26 minutes.<br/>
Once completed you should see the folder `\FO76Edit\Output\` has been populated with 7 CSV files, including one informational CSV noting the debug cells for which extraction was skipped.<br/>
<br/>

### Next steps
Once you have successfully exported the initial data, you can now move on to preprocessing the data, please see [Preprocessor.md](Preprocessor.md)
