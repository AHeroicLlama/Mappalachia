# Rendering map backgrounds

### Prerequisites and assumptions
* You have already [built the database](Preprocessor.md)
* A copy of Fallout 76 installed on your machine
* An installation of Visual Studio
* An installation of [ImageMagick](https://imagemagick.org/script/download.php)
* An installation of [Paint.NET](https://www.getpaint.net/download.html)
* A release of [fo76utils](https://github.com/fo76utils/fo76utils)

### Note
Background image rendering is optional. If you need the images present for debugging, take them from a release. You should only need to run this step if the cells or worldspaces have changed since a game release.

## Setup
At the root of the repository, create the folder `Utilities\fo76utils`. Extract [a release of fo76utils](https://github.com/fo76utils/fo76utils/releases) inside here. The code described here makes calls to the render tool, which does all the heavy lifting for the rendering.<br/>
ImageMagick is used to convert DDS renders to JPG, and for downscaling where required. It is assumed that you followed the normal installation which added image magick to the 'Path' system environment variable.<br/>
Paint.NET is recommended to be installed but is only needed for X/Y coordinate correction, not normal rendering.

## Rendering Cells and Worldspaces
In the main `Mappalachia.sln`, build and run the `BackgroundRenderer` project and select option 1. It will prompt you to press enter to render all spaces, otherwise you may paste a space-separated list of EditorIDs of specific spaces you wish to render.<br/>
The background renderer will connect to the database to identify which spaces should be rendered, therefore the database must be up to date and compiled.<br/>
The background renderer will also need to access Fallout 76 game assets directly from your installation in order to render them. If your Fallout 76 is installed in a non-standard path, edit the `Library.BuildTools.GamePath` string accordingly before building.<br/>
The renderer will use fo76utils to render the images, then ImageMagick to convert them. The outputted files will be placed in the `Assets\img\` folder.<br/>

## Correcting the X/Y offset, zoom, or Z height of renders

### X/Y Offset & Zoom
To ensure quality maps, we must ensure that the view of plotted cells is correctly centered and zoomed onto the view of the cell contents. This cannot always be automatically done, as Bethesda sometimes leave assets far outside the playable area.<br/>

Selecting option 2 in the background renderer will guide you through a CLI wizard which helps correct this. It is recommended to assign Paint.NET as the default program for .jpg files - the tool will generate a quick render of the cell, then ask you to draw a box around the true cell contents, and enter the position and dimensions of that area. Once inputted, the tool will write a file to `BackgroundRenderer\Corrections\XY_Scale\`. This file is read by the Preprocessor, to correct for the positioning of the cell (both its render and its plots.)<br/>

If you have amended a cell this way, you must rebuild the database to incorporate the new correction, and then re-render the cell background.

### Z Crop
Many cells have a roof or ceiling with occludes the view of the cell contents when viewed from above. In order to get the best possible view of the cell, we crop in just below the obstruction, revealing the view of the cell (or at least the top floor, where applicable). This data is not stored in the database, as it is only necessary for the background renderer.<br/>

Select option 3 when running the Background Renderer to enter a wizard to help find and store the correct offset. This process simply consists of estimating the height, rendering an example, and repeating until a good height is found. The correct height once confirmed is stored at `BackgroundRenderer\Corrections\Z\`.<br/>

The wizard will prompt you to re-render the cell properly once the correct Z crop is found.

### Next steps
You may now wish to validate the exported images with the validation functionality of the [Preprocessor](Preprocessor.md), or also optionally [extract the map marker icons](IconExtraction.md) if not already.<br/>
Otherwise, you can now move on to development of the actual [end-user GUI program, Mappalachia](GUI.md).
