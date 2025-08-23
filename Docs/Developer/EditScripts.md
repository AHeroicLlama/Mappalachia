# Running Mappalachia export scripts

### Prerequisites and assumptions
* A copy of Fallout 76 installed on your machine

<br/>

The Pascal scripts found in `\FO76Edit\Edit Scripts\` are built specifically to be used with the Fallout 76 Version of XEdit, also known as FO76Edit.<br/>

### Installing FO76Edit
You should install/unzip a copy of FO76Edit into the `\FO76Edit\` folder.<br/>
FO76Edit is not distributed in this repository and you will need to grab it online.<br/>
The official version of FO76Edit is [available on NexusMods](https://www.nexusmods.com/fallout76/mods/30).<br/>
More regularly updated builds are also available at the [Fallout 76 Datamining Discord](https://discord.com/channels/602253035061903368/824357856958611456).

### Launching FO76Edit
Once in FO76Edit you will be prompted with which ESM to load. Select SeventySix.esm and hit OK. Wait for the message "Background Loader: finished".<br/>
If your copy of Fallout 76 is not installed with Steam in the default location, launch FO76Edit64.exe with the argument `-D:"your_installation_path\Fallout76\Data"`.

### Running the export scripts
Right click on SeventySix.esm in the tree on the left and select 'Apply Script...'.<br/>
Select `_mappalachia_RUNALL` as your script to run.<br/>
This script runs all Mappalachia scripts consecutively and should take around 30 minutes.<br/>
Once completed you should see the folder `\FO76Edit\Output\` has been populated with 8 CSV files.<br/>


### Next steps
Once you have successfully exported the initial data, you can now move on to preprocessing the data, please see [Preprocessor.md](Preprocessor.md)
