﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CommonwealthCartography
{
	static class Preprocessor
	{
		static string inputPath;
		static string outputPath;

		static async Task Main()
		{
			Console.Title = "Commonwealth Cartography Preprocessor";

			// Step back out of bin until we find the CommonwealthCartography root
			string CommonwealthCartographyRoot = AppDomain.CurrentDomain.BaseDirectory;
			while (Path.GetFileName(Path.GetDirectoryName(Path.GetFullPath(CommonwealthCartographyRoot))) != "Commonwealth_Cartography")
			{
				CommonwealthCartographyRoot += "..\\";
			}

			inputPath = CommonwealthCartographyRoot + "//FO4Edit//Output//";
			outputPath = CommonwealthCartographyRoot + "//Preprocessor//Output//";

			try
			{
				// Store all preprocessor tasks in a list
				// Each task represents one CSV file being output
				List<Task> parallelTasks = new List<Task>
				{
					new Task(() => ProcessSpatialFile("Position_Data.csv")),
					new Task(() => ProcessBasicFile("Entity_Info.csv")),
					new Task(() => ProcessBasicFile("Region.csv")),
					new Task(() => ProcessSpaceFile()),
					new Task(() => GenerateNPCSpawnFile()),
					new Task(() => ProcessBasicFile("Junk_Scrap.csv")),
				};

				// Start all tasks
				foreach (Task task in parallelTasks)
				{
					task.Start();
				}

				// Wait for all Tasks to finish
				await Task.WhenAll(parallelTasks.ToArray());

				Console.WriteLine("Done with all! Press any key");
				Console.ReadKey();
			}
			catch (Exception e)
			{
				Console.WriteLine("An error was reported preprocessing the data and the program cannot continue until this is resolved.\n" + e);
				Console.ReadKey();
			}
		}

		// Apply the standard processing to a given coordinate file (Worldspace or interior)
		static void ProcessSpatialFile(string fileName)
		{
			CSVFile file = GenericOpen(fileName);
			file = GenericCSVHelper.DuplicateColumn(file, "shortName", "instanceID");

			// Strip the map markers data out the position file before they are reduced away
			CSVFile mapMarkersFile = MapMarkers.ProcessMapMarkers(file);
			GenericProcess(mapMarkersFile);
			GenericClose(mapMarkersFile);

			GenericProcess(file);
			GenericClose(file);
		}

		// Apply complete cycle of basic processing (open-close)
		static void ProcessBasicFile(string fileName)
		{
			CSVFile file = GenericOpen(fileName);
			GenericProcess(file);
			GenericClose(file);
		}

		// Adds the custom nudge values to the space_info file
		static void ProcessSpaceFile()
		{
			CSVFile file = GenericOpen("Space_Info.csv");

			file = SpaceNudge.AddNudgeToSpaces(file);

			GenericProcess(file);
			GenericClose(file);
		}

		// Process the location CSVFile and then use it to generate a new file for NPCSpawns
		static void GenerateNPCSpawnFile()
		{
			CSVFile locationFile = GenericOpen("Location.csv");
			GenericProcess(locationFile);

			CSVFile npcSpawns = NPCSpawnHelper.ProcessNPCSpawns(locationFile);
			locationFile.rows = null;

			GenericProcess(npcSpawns);
			GenericClose(npcSpawns);
		}

		// Shorthand to instantiate a new CSVFile from file name
		static CSVFile GenericOpen(string fileName)
		{
			return new CSVFile(inputPath + fileName);
		}

		// Apply the standard processing to the CSVFile
		static void GenericProcess(CSVFile file)
		{
			file.Sanitize();
			file.CorrectAnomalies();
			file.ReduceReferences();
			file.ReduceDecimals();
		}

		// Validate the file, write it to disk and free the memory.
		static void GenericClose(CSVFile file)
		{
			file.Validate();
			Directory.CreateDirectory(outputPath);
			file.WriteToFile(outputPath + "//");

			file.rows = null;
			GC.Collect();

			Console.WriteLine(file.fileName + " done.");
		}
	}
}
