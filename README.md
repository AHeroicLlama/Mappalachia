# Mappalachia

The complete mapping tool for Fallout 76.<br/>
Mappalachia is a Windows application for generating and exporting complex maps of entities within the Fallout 76 game world.

[![Latest release](https://img.shields.io/github/downloads/AHeroicLlama/Mappalachia/total)](https://github.com/AHeroicLlama/Mappalachia/releases/latest)<br>
![](https://img.shields.io/github/last-commit/AHeroicLlama/Mappalachia)<br/>
[![Latest release](https://img.shields.io/github/v/release/aheroicllama/mappalachia)](https://github.com/AHeroicLlama/Mappalachia/releases/latest)<br/>
![](https://img.shields.io/badge/game%20version-1.7.21.8-green)<br/>
[![Discord](https://img.shields.io/discord/1029499482028646400?label=Discord&logo=Discord)](https://discord.gg/Z2GMpm6rad)<br/>
[![License](https://img.shields.io/github/license/AHeroicLlama/Mappalachia)](LICENSE.md)

## Download and Installation
[__Download 'Mappalachia.zip' from here__](https://github.com/AHeroicLlama/Mappalachia/releases/latest) to get started generating maps. Simply unzip it to a folder and then launch Mappalachia.exe.<br/>
For help installing please refer to the [installation and first launch guide](Docs/User/Installation.md).<br/>

## User Guides
A number of User guides exist for Mappalachia in document form;<br/>

* [**Installation and first launch**](Docs/User/Installation.md)
* [**Map Basics**](Docs/User/First_map.md)
* [**Map Customization**](Docs/User/Map_customization.md)
* [**Alternative Plot Modes**](Docs/User/Plot_modes.md)
* [**Scrap, NPCs, and Flux**](Docs/User/Derived_results.md)
* [**Space Selection and Misc Advanced Features**](Docs/User/Advanced.md)
* [**Spotlight Mode**](Docs/User/Spotlight.md)
* [**Map Recipes**](Docs/User/Recipes.md)
* [**In-Game Map mod**](Docs/User/Game_map_mod.md)

## Discord
[Join the Mappalachia Discord](https://discord.gg/Z2GMpm6rad) for discussion and help.

## Info for Developers
Alongside the source code for the GUI itself, this repository also contains the necessary scripts and code used to export, preprocess and build the Mappalachia database and also generate/render supporting image assets.

The required information is compiled in 3 key steps.
1. Extract the raw data in CSV using FO76Edit
2. Preprocess the data and assemble the database, with validation
3. Image Asset rendering and extraction

If you fancy doing some data mining or development with Mappalachia then you may be interested in the following documentation;

* [**FO76Edit scripts**](Docs/Developer/EditScripts.md) explains using FO76Edit to run the Mappalachia edit scripts to export rough, raw game data.
* [**Preprocessor**](Docs/Developer/Preprocessor.md) covers using the CLI tool to assemble the database, and provide data and asset validation.
* [**Background Image Rendering**](Docs/Developer/BackgroundRendering.md) explains using the powerful fo76utils to render top-down views of locations, used for map backgrounds.
* [**Map Icon extraction**](Docs/Developer/IconGeneration.md) explains the process of exporting map marker icons from the game to Mappalachia.
* [**GUI**](Docs/Developer/GUI.md) covers developing and debugging the Mappalachia GUI itself.
* [**Deployment**](Docs/Developer/Deployment.md) explains how to update and deploy Mappalachia, including following a game update.

## Thanks
* Every single person who has so generously donated to say thanks for Mappalachia.
* All prominent members of the Mappalachia Discord who have provided feedback and ideas for new features. (Gilpo, Duchess Flame, MeatServo, Scratchy, frame, +rat, fo76utils, pcrov and more)
* Contributors to and developers of XEdit and FO76Edit, namely Eckserah.
* Members of the FO76 Datamining Discord, for helping out with FO76Edit and Edit Scripts.
* [fo76utils](https://github.com/fo76utils) for their excellent, powerful [render tool](https://github.com/fo76utils/fo76utils), used to render background images.

#### Licensing
This project is licensed under the GNU General Public License 3.0 - see [LICENSE.md](LICENSE.md) for details.<br/>
Mappalachia uses technologies such as [.NET](https://dotnet.microsoft.com/en-us/platform/free), [SQLite](https://www.sqlite.org/copyright.html), [SVG.NET](https://github.com/svg-net/SVG?tab=MS-PL-1-ov-file#readme) and [KGy SOFT Core Libraries](https://kgysoft.net/corelibraries/#license) which are each subject to their own licenses.<br/>
Use of other third-party assets are covered below.

#### Legal/Disclaimer
Mappalachia is provided as a non-commercial, free tool solely for the benefit of players of Fallout 76. Mappalachia and its creator are neither affiliated with - nor endorsed by - ZeniMax Media or any of its subsidiaries including Bethesda Softworks LLC. Any and all game data and/or assets including but not limited to images, characters, names and other game data which are contained within this application (and thus stored and distributed at this repository) are extracted from a purchased copy of Fallout 76 and are shared with the player community in good faith and for the explicit purpose of making maps for the benefit of said community, with an understanding that this lies within fair use.<br/>
Great care has been taken to minimize such data so that it cannot be reconstructed in any meaningful way.<br/>
If you have any concerns or queries, please direct them to mappalachia.feedback@gmail.com.
