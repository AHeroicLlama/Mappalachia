# Extracting Map Marker icons

### Prerequisites and assumptions
* You have already [built the database](Ingest.md)
* A copy of [Besthesda Archive Extractor](https://www.nexusmods.com/fallout4/mods/78/) (BAE)
* An installation of [JPEXS Free Flash Decompiler](https://github.com/jindrapetrik/jpexs-decompiler/releases/latest)
	* Which will itself require [Java](https://www.java.com/en/download/)
* An installation of Visual Studio

### Note
Map marker extraction is optional, as map marker images are already stored in the repository. You should only need to run this step if you believe the map marker icons have changed since a game release. The database summary will indicate if any new markers are used, but not if any marker images have themselves changed. The [Image Asset Validator](ImageAssetValidation.md) will also identify that all required files are present.

## Process overview
Fallout 76 uses SWF (Flash) to power most if its UIs, this includes the map menu and markers inside.<br/>
This stage ensures Mappalachia has all of the required SVG images to render map marker icons if the user enables that setting.<br/>
We first extract the SWF menu files from the game, and then decompile those to find the stored map marker icons inside, then rename and move them, verifying all icons are accounted for.

## Extracting the SWFs
Launch BAE and hit 'File' > 'Open File', navigate to your Fallout 76 installation path, and its `data` folder. Select the two files: `SeventySix - Interface.ba2` and `SeventySix - Interface_en.ba2`.<br/>
Press 'extract' and select any temporary output folder. BAE should extract 100+ SWF files.<br/>

## Extracting SVG sprites from the SWF
In your files extracted from BAE, under the `interface` folder, find the file `mapmenu.swf`. Open this file with JPEXS. Some questions may appear, select 'Yes to all'.<br/>
In JPEXS, on the left hand side displaying the contents of `mapmenu.swf`, select the `sprites` folder.<br/>
Under File > Export, select 'Export selection'. Ensure the file type is set as SVG, and the zoom is at 100%.<br/>
Press OK and select the output location as `Mappalachia\MapIconProcessor\extract\`.<br/>
Some errors may appear, select 'Ignore All'. The bottom-left of the window will indicate when the extract has finished.<br/>
This should now populate a `sprites` folder filled with around 500 sub-folders all prefixed "DefineSprite".

## MapIconProcessor
Now in `Mappalachia.sln`, compile and run the `MapIconProcessor` project.<br/>
This is a small program whose job is to copy only the necessary map marker icon images, rename them suitably, and verify that all icons requested by the database are accounted for.<br/>
If there are any missing or unavailable marker images, this will be raised as an error in the console output, and additionally a file will be written to the repository to ensure it cannot be missed.<br/>
The MapIconProcessor accesses the database to identify which map markers are required, so you are required to have built the database in order to run it.<br/>
The processor will then use RegEx to find the map markers in their folders, rename them and copy them to the main Mappalachia GUI at `Mappalachia\Mappalachia\img\mapmarker\`<br/>

### Next steps
You may wish to validate the exported images with the [Image Asset Validator](ImageAssetValidation.md), or also optionally [render the background images](BackgroundRendering.md) if not already.<br/>
Otherwise, you can now move on to development of the actual [end-user GUI program, Mappalachia](GUI.md).
