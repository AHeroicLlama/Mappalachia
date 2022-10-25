# Rendering map backgrounds

### Prerequisites and assumptions
* You have already [built the database](Ingest.md)
* A copy of Fallout 76 installed on your machine
* An installation of Visual Studio
* An installation of [ImageMagick](https://imagemagick.org/script/download.php)
* A release of [fo76utils](https://github.com/fo76utils/fo76utils), specifically the render utility

### Note
Background image rendering is optional. If you need the images present for debugging, take them from a release. You should only need to run this step if you believe the cells or worldspaces have changed significantly since a game release. It is not recommended to re-render all spaces unless completely necessary. The database summary will indicate if a space has changed significantly, and the [Image Asset Validator](ImageAssetValidation.md) will identify that all required files are at least present.

## Setup
At the root of the repository (beside the `readme.md`), create the folder `FO76Utils\`. Extract [a release of fo76utils](https://github.com/fo76utils/fo76utils/releases) inside here. We only need `render.exe` and the 3 .DLLs prefixed 'lib' which it requires. You can delete any other files if you wish. The code described here makes calls to this render tool, which does all the heavy lifting for the rendering.<br/>
ImageMagick is used to convert DDS renders to JPG, and for downscaling where required. In the `Render_Appalachia.bat`, and the Background Render project, the path to the current installation of ImageMagick is hardcoded. You may need to adjust these paths to target your installation version.<br/>

## Rendering Appalachia
In the `BackgroundRenderer\` folder, execute `Render_Appalachia.bat`. Once completed, the rendered Appalachia jpg will be placed at the expected location for the GUI to find it.<br/>Note: This render takes *a lot* of computing power. (16k at 2x SSAA downscaled to 4k). If necessary, edit the script to render straight to 4k.<br/>

## Rendering all Cells
In the main `Mappalachia.sln`, build and run the `BackgroundRenderer` project. It will prompt you to press enter to render all cells, otherwise you may paste a space-separated list of EditorIDs of specific cells you wish to render.<br/>
View the `summary.txt` from the database build process in git to easily find new EditorIDs.<br/>
The background renderer will connect to the database to identify which cells should be rendered, therefore the database must be up to date and compiled.<br/>
The background renderer will also need to access Fallout 76 game assets directly from your installation in order to render them. If your Fallout 76 is installed in a non-standard path, edit the `fo76DataPath` string accordingly before building.<br/>
The renderer will use fo76utils to render the images, then ImageMagick to convert them. The outputted files will be placed directly in the `img/cell/` folder.<br/>

Note: If there are errors in the batch process, a file `errors.txt` will be written describing what happened.

### Next steps
You may now wish to validate the exported images with the [Image Asset Validator](ImageAssetValidation.md), or also optionally [extract the map marker icons](IconExtraction.md) if not already.<br/>
Otherwise, you can now move on to development of the actual [end-user GUI program, Mappalachia](GUI.md).
