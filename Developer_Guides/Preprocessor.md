# Preprocessing the exported data

### Prerequisites and assumptions
* You have already run the [export scripts](EditScripts.md) using FO76Edit
* An installation of Visual Studio
* (Optional) competency with C# and .NET

## What is the Preprocessor?
The Mappalachia Preprocessor is a simple C# .NET CLI tool which preprocesses the data in the initial exported CSVs into the format required for the database.<br/>
It carries out several key steps;
* Data validation, based on expected values for each column.
* Data minimization, removing junk data and excess precision, leaving the bare minimum required for Mappalachia to function.
* Combination and transformation of certain tables - notably the final Junk and NPC tables are each formed from a combination of data points from originally exported CSVs.
* It also injects position "nudges" to the CSV for the Space_Info table

## How to use the Preprocessor
You simply need to build and run the preprocessor exe. There are no arguments or inputs required. The preprocessor assumes you have run the export scripts and have left the outputted CSVs where they were exported to.<br/>
To do this, find the Preprocessor project inside the main Mappalachia solution at `Mappalachia.sln`, then build and start the `Preprocessor` project.<br/>
Much like the export scripts, once complete the preprocessor will generate a new folder, `\Preprocessor\Output\` which contains 6 preprocessed CSVs.<br/>
If any issues arise (most likely due to failing validation, after a new game update changes something), they will be reported to the console via a raised Exception.<br/>
*Please note: Due to being heavily parallelized, the preprocessor tends to allocate around 8GB memory (and is therefore configured as 64-bit).*

### Next steps
Once the data has been preprocessed, it can now be ingested into the database, please see [Ingest.md](Ingest.md)
