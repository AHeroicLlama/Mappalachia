# Extracting Map Marker icons

### Prerequisites and assumptions
* You have already [built the database](Preprocessor.md)
* A copy of [Besthesda Archive Extractor](https://www.nexusmods.com/fallout4/mods/78/) (BAE)
* An installation of [JPEXS Free Flash Decompiler (FFDec)](https://github.com/jindrapetrik/jpexs-decompiler/releases/latest)
	* Which will itself require [Java](https://www.java.com/en/download/)
* An installation of Visual Studio

### Note
Map marker extraction is optional, as map marker images are already stored in the repository. You should only need to run this step if you believe the map marker icons have changed since a game release. The image validation functionality of the Preprocessor will highlight if any necessary icon files are missing, but not if they have changed.

### Door Marker
Separately from the map markers, there is another icon used for indicating items in other spaces, the door marker. The Map Icon Processor does not interact with this file, and it is not present in the database, as it is not associated with any coordinates in-game, and is not expected to ever change. If you do need to re-extract it, it can be found at `C:\Program Files (x86)\Steam\steamapps\common\Fallout76\Data\SeventySix - Interface.ba2\interface\mapmarkerlibrary.swf\sprites\DefineSprite (442)`. These files can be opened and extracted from by using the process described in this document.

## Process overview
Fallout 76 uses SWF (Flash) to power most if its UIs, this includes the map menu and markers inside.<br/>
This stage ensures Mappalachia has all of the required SVG images to render map marker icons if the user enables that setting.<br/>
We first extract the SWF menu files from the game, and then decompile those to find the stored map marker icons inside, then rename and move them, verifying all icons are accounted for.

## Extracting the SWFs
Launch BAE and hit 'File' > 'Open File', navigate to your Fallout 76 installation path, and its `data` folder. Select the two files: `SeventySix - Interface.ba2` and `SeventySix - Interface_en.ba2`.<br/>
Press 'extract' and select any temporary output folder. BAE should extract 100+ SWF files.<br/>

## Extracting SVG sprites from the SWF
In your files extracted from BAE, under the `interface` folder, find and open the file `mapmarkerslibrary.swf` with JPEXS.<br/>
Ensure you open `mapmarkerslibrary.swf`, not the similarly named `mapmarkerlibrary.swf`.<br/>
Some questions may appear, select 'Yes to all'.<br/>
In JPEXS, on the left hand side displaying the contents of `mapmarkerslibrary.swf`, select the `sprites` folder.<br/>
Under File > Export, select 'Export selection'. Ensure the file type is set as SVG, and the zoom is at 100%.<br/>
Press OK and select the output location inside the cloned repository at `MapIconProcessor\extract\`.<br/>
If errors appear, select 'Ignore All'. The bottom-left of the window will indicate when the extract has finished.<br/>
This should now populate a `sprites` folder filled with around 200 sub-folders all prefixed "DefineSprite".

## MapIconProcessor
Now in `Mappalachia.sln`, compile and run the `MapIconProcessor` project.<br/>
This is a small program whose job is to extract the necessary map marker icon images, rename, trim, and validate them.<br/>

Similarly to the preprocessor, if there are any errors these will be outputted to the console, and written to `BuildOutputs\Errors.txt`.<br/>
The MapIconProcessor accesses the database to identify which map markers are required, so you are required to have built the database in order to run it.<br/>

Final map marker icons will be placed at `Assets\img\mapmarker\`.

### Next steps
You may wish to validate the exported images with the [Preprocessor](Preprocessor.md), or also optionally [render the background images](BackgroundRendering.md) if not already.<br/>
Otherwise, you can now move on to development of the actual [end-user GUI program, Mappalachia](GUI.md).
