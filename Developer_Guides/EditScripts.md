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
There are a few versions of FO76Edit available. The 'official' version is [available on NexusMods](https://www.nexusmods.com/fallout76/mods/30). However as of writing this version has not received updates for over a year.
For a more up to date version, user Eckserah has spent significant time maintaining an up to date version mostly in line with Bethesda's patches. This version is [available via NukaCrypt](https://nukacrypt.com/ecksedit/latest).<br/>
*Please note these are third party links. While I personally trust them, I can provide no guarantees.*<br/>
<br/>
Once you have downloaded FO76Edit, you should simply be able to dump the contents of the zip into the `\FO76Edit\` folder. (You will note that the `Edit Scripts\` folder is already in place, populated with Mappalachia scripts).<br/>

### Launching FO76Edit
If you installed Fallout 76 through Steam, or installed Fallout 76 in a non-default directory, you will need to launch FO76Edit with a launch parameter `-D:"<Path_to_FO76_Data_folder>"`.<br/>
If you installed Fallout 76 via Bethesda.net and installed in the default location, you shouldn't need to take any extra steps.<br/>
Once in FO76Edit you will be prompted with which ESM to load. Select SeventySix.esm and hit OK. Wait for the background loader to finish.<br/>

### Running the Mappalachia export scripts
Expand SeventySix.esm in the tree on the left. Right click any element and select 'Apply Script...'.<br/>
Select `_mappalachia_RUNALL` as your script to run.<br/>
This script runs all Mappalachia scripts consecutively and should take approximately 26 minutes.<br/>
Once completed you should see the folder `\FO76Edit\Output\` has been populated with 7 CSV files, including one informational CSV noting the debug cells for which extraction was skipped.<br/>
<br/>

### Next steps
Once you have successfully exported the initial data, you can now move on to preprocessing the data, please see [Preprocessor.md](Preprocessor.md)
