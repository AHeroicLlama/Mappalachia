# Using a Mappalachia map in-game

## Intro
While the intended use for Mappalachia while playing is from a second monitor, another device, or by alt-tabbing - it is possible for PC players to replace the in-game map with their own Mappalachia map.

## Prerequisites
* An installation of [Mappalachia](..\\..\readme.md#download-and-installation)
* An installation of [Paint.NET](https://www.getpaint.net/download.html#download)
* An installation of [Baka File Tool](https://www.nexusmods.com/fallout76/mods/9)
* Moderate computer literacy - this guide gets a little technical.

## Generating the map
In Mappalachia, create the map you want to see in-game. When you're ready, go to 'Map' > 'Save Image'.<br>
Export the map image file to any location. (Maximum quality settings are advised (PNG)).

Note: Ensure you have not used 'Extended' legend style, as the final image must be the same dimensions as the normal map texture.

## Creating the DDS
If you wish to make any further manual edits to the map image, you should do this now.

In any folder such as your desktop, create a new folder called `data`. Inside this, create another called `textures`, then inside that, create another called `interface`, and finally inside that create another called `pip-boy`.

Go to the map file you just exported, and open it in Paint.NET.<br>
Go to 'File' > 'Save As...'. Select the target export folder as the `pip-boy` folder. Set the file type to 'DirectDraw Surface (DDS)'. Set the file name to `papermap_city_d.dds` and save. Leave the other export settings as default.

You should now have a directory structure which is exactly `data\textures\interface\pip-boy\papermap_city_d.dds`.

## Creating the mod archive
Now open Baka File Tool (`BakaFileTool.exe` once unzipped).

On the Paths tab, set the 'Fallout 76 Path' as your Fallout 76 PTS install location. This is by default `C:\Program Files (x86)\Steam\steamapps\common\Fallout 76 Playtest\`.<br>
Back on the Tool tab, change the selected Game Mode to Fallout 76.<br>
Under 'Manual Data Folder' tick the 'Enabled' checkbox, then 'Select Data Folder'. Choose the `data` folder which you created earlier.

Now press 'Create Archive'. Your mod archive file will be generated beside the exe at `BakaOutput\Fallout 76\BakaFile - Textures.ba2`.<br>
Rename the ba2 file as you wish.

## Installing the mod
The process is now the same as installing any mod for Fallout 76.

Navigate to the 'Data' directory of your Fallout 76 PTS installation, default `C:\Program Files (x86)\Steam\steamapps\common\Fallout 76 Playtest\Data`.<br>
Place your .ba2 file in this folder.

Navigate to your Fallout 76 settings directory. By default this is `C:\Users\%USERNAME%\Documents\My Games\Fallout 76`.

If you already have the file `Fallout76Custom.ini` here, open it in any text editor (such as notepad or similar). Otherwise, start a new file in a text editor and save it here with that name.

In this file, enter the line `sResourceArchive2List=filename.ba2` where 'filename' is the name of your exported ba2.<br>
If you already have this line present, instead append your ba2 to the end of the line, separated by a comma, for example `sResourceArchive2List=mod1.ba2,filename.ba2`.

Save this file. You can now launch Fallout 76, and find your in-game map replaced.
