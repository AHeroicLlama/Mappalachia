# Spotlight
Spotlight is a powerful optional feature which allows you to zoom in to satellite views at great detail.<br/>
The detail of the world and thus the accuracy of the plots becomes much clearer, without the final image changing size.

## Installing Spotlight

Due to the large file sizes, spotlight is not included by default, but you can choose to download and install it by following these steps:<br/>
* If you have not already, [install 7-Zip](https://www.7-zip.org/download.html). 7-Zip is a popular tool for creating and opening compressed files.
* Go to the special [spotlight release](https://github.com/AHeroicLlama/Mappalachia/releases/tag/spotlight) on GitHub.
* Under assets, download all files named `spotlight.7z.xxx` where xxx are numbers 001 and up.
* Once all files are downloaded, move them to your Mappalachia install location, into the folder `img\`.
* Right click the file `spotlight.7z.001`, and select `7-Zip` > `Extract Here`.
* 7-Zip will unpack all the spotlight images, creating the folder `spotlight\` inside of `img\`.
* Once 7-Zip has completed, you are ready to use the Spotlight feature.
* You may delete all the files you downloaded, leaving the new folder remaining.

## Updating Spotlight
If you already have the latest version of spotlight installed, you may download only newly updated parts of it for the current patch by downloading `patch_spotlight.7z.XXX` files from the [latest release](https://github.com/AHeroicLlama/Mappalachia/releases/latest) (Note if there are only few changed files, there will instead be a single `patch_spotlight.zip`), and similarly unzipping them into the `img\` folder, merging with the `spotlight\` folder you already have there.<br/>
These files will always only contain spotlight data which changed between this release and the prior.<br/>
If you don't have the latest spotlight installed, you may still update it by repeating this process for each release you're missing, oldest to newest.

## Using Spotlight
To use spotlight, simply right-click the map preview at the location you would like and select 'Spotlight Here'. Mappalachia will render the selected area in super high detail. You may amend the effective level of detail by right-clicking again with spotlight on, and selecting 'Set Spotlight Size...'. (Note that using large spotlight sizes can be slow.)<br/>

You may also turn spotlight on/off and set the size at 'Map' > 'Spotlight', however you must use the map preview to select the location of the spotlight.<br/>

If your spotlight maps are blurry and/or without increased clarity, this indicates you have selected an area outside the playable space, otherwise spotlight may be incorrectly installed.

## Next guide
[Recipes](Recipes.md)
