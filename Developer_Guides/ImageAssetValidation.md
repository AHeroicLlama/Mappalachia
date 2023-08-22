# Image asset validation

### Prerequisites and assumptions
* You have already [built the database](Ingest.md)

## Intro
This tool provides safety checks to identify if all required image assets (contents of the `\Mappalachia\img\` folder). Are present and correct.<br/>
The tool doesn't produce any output but should be run prior to any Mappalachia release.

## Asset Validation
In the main `Mappalachia.sln`, build and run the `ImageAssetChecker` project.<br/>
The tool will connect to the database so it know which cells, worldspaces, and map markers are required.<br/>
It will then cross reference to the contents of the `img` folder to verify that each space has an applicable background image file, that the image is of the correct dimensions, and that the file size looks correct. Then it will check for all map markers, also verifying that they exist and that they are of a normal file size.<br/>
Finally the tool will verify there are no extraneous image files present.<br/>

If it detects any errors, these will be logged and again summarized once the check has been completed. In addition, `errors.txt` will be placed in `\Mappalachia\img\` containing the reported errors.

### Next steps
With all image assets validated, you can now move on to development of the actual [end-user GUI program, Mappalachia](GUI.md).
