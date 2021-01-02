# Mappalachia

The complete mapping tool for Fallout 76.<br/>
Mappalachia is a Windows Forms GUI powered by an SQLite backend, used for generating and exporting complex maps of entities within the Fallout 76 world.

The project also comes with the necessary scripts and code used to export, preprocess and build the database, should you wish to do that yourself.

![GitHub](https://img.shields.io/github/last-commit/AHeroicLlama/Mappalachia)<br/>
[![GitHub](https://img.shields.io/badge/mappalachia%20version-1.0.0.0-green)](https://github.com/AHeroicLlama/Mappalachia/release)<br/>
![GitHub](https://img.shields.io/badge/game%20version-1.5.1.3-green)<br/>
[![GitHub](https://img.shields.io/github/license/AHeroicLlama/Mappalachia)](LICENSE.md)<br/>

## Installation

To get started generating maps with Mappalachia, you simply need to [grab the latest release zip](https://github.com/AHeroicLlama/Mappalachia/releases/latest), unzip it to a folder and launch Mappalachia.exe.<br/>
For help installing please refer to the [installation and first launch guide](Guides_user/InstallationFirstRun.md).<br/>


## Getting started - User guides

A number of user guides exist for Mappalachia in document form;<br/>

* [**Installation and First launch**](Guides_user/InstallationFirstRun.md) covers initial installation and getting Mappalachia running.
* [**First map**](Guides_user/FirstMap.md) covers all the basic steps to creating your first Mappalachia map.
* [ **Advanced Search**](Guides_user/AdvancedSearch.md) covers the Intelligent NPC and Scrap search functions, as well as using filters to find exactly what you need.
* [**Advanced Mapping**](Guides_user/AdvancedMapping.md) covers the powerful heatmap mode, as well as item grouping, volume mapping and icon customisation.
* [ **Map Settings**](Guides_user/MapSettings.md) covers the ways you can adjust the visuals of the map, including layers for Nuclear Winter.

## Extract your own data or get stuck in to development

Behind the scenes, Mappalachia is developed and produced in 4 key steps.
1. Extract the raw data in CSV using FO76Edit
2. Refine and preprocess the data
3. Ingest the data into a database
4. Development of the actual GUI/Program itself

If you fancy doing some data mining, or want to do your own thing with the raw data, then you may be interested in the following documentation;

* [**FO76Edit scripts**](Guides_dev/EditScripts.md) covers using FO76Edit to run the Mappalachia edit scripts to export rough, raw game data.
* [**Preprocessor**](Guides_dev/Preprocessor.md) covers compiling and using the CLI tool to process and refine the rough data into proper CSVs.
* [**Ingest**](Guides_dev/Ingest.md) covers using SQLite to ingest the CSVs into a database which Mappalachia can read.
* [**GUI**](Guides_dev/GUI.md) covers developing the Mappalachia GUI itself.


## Thanks

* Contributors to and developers of XEdit and FO76Edit, namely Eckserah.
* Members of the FO76 Datamining Discord, for helping out with FO76Edit and Edit Scripts, and offering valuable knowledge and feedback based on their own experiences datamining and creating Fallout 76 maps.
* Everyone who ever gave feedback to [the original Mappalachia](https://www.reddit.com/r/fo76/comments/bmwpx9/mappalachia_my_project_which_can_automatically/). Your feedback, comments, questions, and PMs were essential to defining and guiding the features I have been able to bring to life here.

#### License

This project is licensed under the GNU General Public License 3.0 - see [LICENSE.md](LICENSE.md) for details.<br/>
Third-party assets and technologies, including but not limited to game data, images and information exported from Fallout 76, and also technologies such as SQLite are each subject to their own terms.

#### Legal/Disclaimer

Mappalachia is provided as a non-commercial, free tool solely for the benefit of players of Fallout 76. Mappalachia and its creator are neither affiliated with - nor endorsed by - ZeniMax Media or any of its subsidiaries including Bethesda Softworks LLC. Game assets including but not limited to images, characters, names and other core game data used for mapping are extracted from a purchased copy of Fallout 76 and are shared here with the game's community in good faith.<br/>
Any game data shared here is done so with the explicit purpose of making maps for the benefit of the community, and great care has been taken to minimise such data so that it cannot be reconstructed in any meaningful way.<br/>
If you have any concerns or queries, please direct them to mappalachia.feedback@gmail.com