# Mappalachia

The complete mapping tool for Fallout 76.<br/>
Mappalachia is a Windows application for generating and exporting complex maps of entities within the Fallout 76 world.

[![GitHub all releases](https://img.shields.io/github/downloads/AHeroicLlama/Mappalachia/total)](https://github.com/AHeroicLlama/Mappalachia/releases/latest)<br>
![GitHub](https://img.shields.io/github/last-commit/AHeroicLlama/Mappalachia)<br/>
[![GitHub](https://img.shields.io/github/v/release/aheroicllama/mappalachia)](https://github.com/AHeroicLlama/Mappalachia/releases/latest)<br/>
![GitHub](https://img.shields.io/badge/game%20version-1.6.1.18-green)<br/>
[![GitHub](https://img.shields.io/github/license/AHeroicLlama/Mappalachia)](LICENSE.md)<br/>

## Download and Installation

[__Download Mappalachia.zip here__](https://github.com/AHeroicLlama/Mappalachia/releases/latest) to get started generating maps. Simply unzip it to a folder and then launch Mappalachia.exe.<br/>
For help installing please refer to the [installation and first launch guide](User_Guides/Installation_and_first_run.md).<br/>

## Getting started - User Guides

A number of User guides exist for Mappalachia in document form;<br/>

* [**Installation and First run**](User_Guides/Installation_and_first_run.md) covers initial installation and getting Mappalachia running.
* [**First map**](User_Guides/First_map.md) covers all the basic steps to creating your first Mappalachia map.
* [**Advanced Searching**](User_Guides/Advanced_searching.md) explains the Intelligent NPC and Scrap search functions, as well as using filters to find exactly what you need.
* [**Advanced Mapping**](User_Guides/Advanced_mapping.md) details the powerful heatmap mode, as well as topographical plotting, item grouping, volume mapping and icon customization.
* [**Map Settings**](User_Guides/Map_settings.md) covers the ways you can adjust the visuals of the map.
* [**Cell Mode**](User_Guides/Cell_mode.md) explains how to use the advanced Cell mode to plot maps of interiors.
* [**Updating or Uninstalling**](User_Guides/Updating_or_uninstalling.md) describes how to update or remove Mappalachia.

## Info for Developers

Alongside the source code for the GUI itself, this repository also contains the necessary scripts and code used to export, preprocess and build the Mappalachia database.

The database is developed and produced in 3 key steps.
1. Extract the raw data in CSV using FO76Edit
2. Refine and preprocess the data
3. Ingest the data into a database

If you fancy doing some data mining or development with Mappalachia then you may be interested in the following documentation;

* [**FO76Edit scripts**](Developer_Guides/EditScripts.md) covers using FO76Edit to run the Mappalachia edit scripts to export rough, raw game data.
* [**Preprocessor**](Developer_Guides/Preprocessor.md) covers compiling and using the CLI tool to process and refine the rough data into proper CSVs.
* [**Ingest**](Developer_Guides/Ingest.md) covers using SQLite to ingest the CSVs into a database which Mappalachia can read.
* [**GUI**](Developer_Guides/GUI.md) covers developing the Mappalachia GUI itself.


## Thanks

* Every single person who has so generously donated to say thanks for Mappalachia.
* Contributors to and developers of XEdit and FO76Edit, namely Eckserah.
* Members of the FO76 Datamining Discord, for helping out with FO76Edit and Edit Scripts, and offering valuable knowledge and feedback based on their own experiences datamining and creating Fallout 76 maps.
* Gilpo for providing great ideas and feedback for new Mappalachia features.
* Everyone who ever gave feedback to the original Mappalachia. Your feedback, comments, questions, and PMs were essential to defining and guiding the features I have been able to bring to life here.

#### License

This project is licensed under the GNU General Public License 3.0 - see [LICENSE.md](LICENSE.md) for details.<br/>
Third-party assets and technologies, including but not limited to game data, images and information exported from Fallout 76, and also technologies such as SQLite are each subject to their own terms.

#### Legal/Disclaimer

Mappalachia is provided as a non-commercial, free tool solely for the benefit of players of Fallout 76. Mappalachia and its creator are neither affiliated with - nor endorsed by - ZeniMax Media or any of its subsidiaries including Bethesda Softworks LLC. Game assets including but not limited to images, characters, names and other core game data used for mapping are extracted from a purchased copy of Fallout 76 and are shared here with the game's community in good faith.<br/>
Any game data shared here is done so with the explicit purpose of making maps for the benefit of the community, and great care has been taken to minimize such data so that it cannot be reconstructed in any meaningful way.<br/>
If you have any concerns or queries, please direct them to mappalachia.feedback@gmail.com
