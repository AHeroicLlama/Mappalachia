# Rendering map backgrounds

### Prerequisites and assumptions
* You have already [built the database](Ingest.md)
* A copy of Fallout 76 installed on your machine
* An installation of Visual Studio
* An installation of [Paint.NET](https://www.getpaint.net/download.html)
* A release of [fo76utils](https://github.com/fo76utils/fo76utils), specifically the render utility

### Note
Background image rendering is optional, as background images are already stored in the repository. You should only need to run this step if you believe the cells or worldspaces have changed significantly since a game release. It is not recommended to re-render all spaces unless completely necessary. The database summary will indicate if a space has changed significantly, and the [Image Asset Validator](ImageAssetValidation.md) will identify that all required files are at least present.

## Setup
At the root of the repository (beside the `readme.md`), create the folder `FO76Utils\`. Extract [a release of fo76utils](https://github.com/fo76utils/fo76utils/releases) inside here. We only need `render.exe` and the 3 .DLLs prefixed 'lib' which it requires. You can delete any other files if you wish.<br/>
The following steps make calls to this render tool, which does all the heavy lifting for the rendering.

## Rendering Appalachia
In the `BackgroundRenderer\` folder, execute `Render_Appalachia.bat`.<br/>
If you're only updating the Appalachia map, you can skip ahead to 'Converting to JPG'.

## Rendering all Cells
In the main `Mappalachia.sln`, build and run the `BackgroundRenderer` project. It will prompt you to press enter to render all cells, otherwise you may paste a space-separated list of FormIDs of specific cells you wish to render.<br/>
View the `summary.txt` from the database build process in git to easily find new FormIDs.<br/>
The background renderer will connect to the database to identify which cells should be rendered, therefore the database must be up to date and compiled.<br/>
The background renderer will also need to access Fallout 76 game assets directly from your installation in order to render them. If your Fallout 76 is installed in a non-standard path, edit the `fo76DataPath` string accordingly before building.<br/>
The render process is very computationally expensive and slow, expect rendering all cells to take multiple hours.<br/>
Rendered images will be placed at `\Mappalachia\img\*.dds`<br/>
<br/>
Note: It is expected that a few cells are required to be exported at 4096 resolution due to their already tiny size. These will be reported in an `errors.txt` file. If you need them exporting, you will need to manually adjust the `resolution` int and re-run the export for the listed cells.

## Converting to JPG
Images rendered by the fo76utils render tool are exported in .DDS format, however Mappalachia requires them to be in .JPG.<br/>
By default, the Background Renderer renders in 16k, so that we can now supersample the images. If you instead modified the renderer to only export in 4k, then you should skip the resizing step below.<br/>

For each of your exported DDS map images - take the following steps;
- Open the image in Paint.NET
- Resize the image (Ctrl+R)
    - By percentage: 25%
- Save as (Ctrl+Shift+S)
- Select the `cell` folder
- Change the file type to JPEG (*.jpg)
- Set the quality to 85%

25% resize is so the 16k image is sampled down to 4k.<br/>
85% quality is a balance between good quality and acceptable download size (given there are around 150 cell background images).<br/>
If you are converting many images, this process could be automated with VBScript or AHK.

### Next steps
You may now wish to validate the exported images with the [Image Asset Validator](ImageAssetValidation.md), or also optionally [extract the map marker icons](IconExtraction.md) if not already.<br/>
Otherwise, you can now move on to development of the actual [end-user GUI program, Mappalachia](GUI.md).
