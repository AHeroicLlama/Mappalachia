# Preprocessing the exported data

### Prerequisites and assumptions
* You have already run the [export scripts](EditScripts.md) using FO76Edit
* An installation of Visual Studio 2019
* Familiarity with Visual Studio or similar IDEs
* (Optional) competency with C# and .Net

## What is the Preprocessor?
The Mappalachia Preprocessor is a simple C# .Net CLI tool which preprocesses the data in the initial exported CSVs into the format required for the database.<br/>
It carries out several key steps;
* Data validation, based on expected values for each column.
* Data minimisation, removing junk data and excess precision, leaving the bare minimum required for Mappalachia to function.
* Combination and transformation of certain tables - notably the final Junk and NPC tables are each formed from a combination of data points from originally exported CSVs

## How to use the Preprocessor
You simply need to build and run the preprocessor exe. There are no arguments or inputs required. The preprocessor assumes you have run the export scripts and have left the outputted CSVs where they were exported to.<br/>
The preprocessing should take around 10 seconds to complete and (much like the export scripts) will also generate a new folder, `\Preprocessor\Output\` in which the 5 preprocessed CSVs will be placed.<br/>
If any issues arise (most likely due to failing validation, after a new game update changes something), they will be reported to the console via a raised Exception.

### Next steps
Once the data has been preprocessed, it can now be ingested into the database, please see [Ingest.md](Ingest.md)
