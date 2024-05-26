# Using a Mappalachia map in-game

## Intro
While the intended use for Mappalachia while playing is from a second monitor, another device, or by alt-tabbing - it is possible for PC players to replace the in-game map with their own Mappalachia map.

## Prerequisites
* An installation of [Mappalachia](..\readme.md#download-and-installation)
* An installation of [Paint.NET](https://www.getpaint.net/)
* An installation of [Fallout 4 Creation Kit](https://store.steampowered.com/app/1946160/Fallout_4_Creation_Kit/)
* Moderate computer literacy - this guide gets a little technical.

## Generating the map
In Mappalachia, create the map you want to see in-game. When you're ready, go to 'Map' > 'Export To File...'.<br>
Export the map image file to any location. (Maximum quality settings are advised (PNG)).

## Creating the DDS
If you wish to make any further manual edits to the map image, you should do this now.

In any folder such as your desktop, create a new folder called `textures`. Inside this, create another called `interface`, and finally inside that create another called `pip-boy`.

Go to the map file you just exported, and open it in Paint.NET.<br>
Go to 'File' > 'Save As...'. Select the target export folder as the `pip-boy` folder. Set the file type to 'DirectDraw Surface (DDS)'. Set the file name to `papermap_city_d.dds` and save. Leave the other export settings as default.

You should now have a directory structure which is exactly `textures\interface\pip-boy\papermap_city_d.dds`.

## Creating the mod archive
Navigate to `Archive2.exe` under where you installed the FO4 Creation Kit. By default this is `C:\Program Files (x86)\Steam\steamapps\common\Fallout 4\Tools\Archive2\Archive2.exe` and launch it.

Go to 'Archive' > 'Add Folder...' and select the `textures` folder which you created earlier.<br>
Next go to 'Archive' > 'Settings...' and change the 'Format' to DDS.

Now go to 'File' > 'Save As...', and export your mod as a ba2 file with any name.<br>
If you want to share your mod, this is the file you should share.

## Installing the mod
The process is niw the same as installing any mod for Fallout 76

Navigate to the 'Data' directory of your Fallout 76 installation, default `C:\Program Files (x86)\Steam\steamapps\common\Fallout76\Data`.<br>
Place your .ba2 file in this folder.

Navigate to your Fallout 76 settings directory. By default this is `C:\Users\%USERNAME%\Documents\My Games\Fallout 76`.

If you already have the file `Fallout76Custom.ini` here, open it in any text editor (such as notepad/notepad++). Otherwise, start a new file in any text editor and save it here with that name.

In this file, enter the line `sResourceArchive2List=filename.ba2` where 'filename' is the name of your exported ba2.<br>
If you already have this line present, instead append your ba2 to the end of the line, separated by a comma, for example `sResourceArchive2List=mod1.ba2,filename.ba2`.

Save this file. You can now launch Fallout 76, and find your in-game map replaced.

