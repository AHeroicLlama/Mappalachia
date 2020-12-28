using System;
using System.IO;
using System.Threading.Tasks;

namespace Mappalachia
{
	static class Preprocessor
	{
		//These paths assume the repo structure. They also assume you've already ran the XEdit scripts.
		//Change as necessary.
		static readonly string inputPath = AppDomain.CurrentDomain.BaseDirectory + "..//..//..//FO76Edit//Output//";
		static readonly string outputPath = AppDomain.CurrentDomain.BaseDirectory + "..//..//Output";

		static void Main()
		{
			try
			{
				//The worldspace file takes longer to process than everything else combined. So start it going in its own thread
				Task processWorldspace = new Task(() => ProcessSpatialFile("SeventySix_Worldspace.csv"));
				processWorldspace.Start();

				ProcessSpatialFile("SeventySix_Interior.csv");
				ProcessBasicFile("SeventySix_FormID.csv");
				GenerateNPCSpawnFile();
				GenerateQuantifiedJunkScrapFile();

				processWorldspace.Wait();
				Console.WriteLine("Done with all! Press any key");
				Console.ReadKey();
			}
			catch (Exception e)
			{
				Console.WriteLine("An error was reported Pre-Processing the data and the program cannot continue until this is resolved.\n" + e);
				Console.ReadKey();
			}
		}

		//Apply the standard processing to a given coordinate file (Worldspace or interior)
		static void ProcessSpatialFile(string fileName)
		{
			CSVFile file = GenericOpen(fileName);
			file = NpcSpawnHelper.AddMonsterClassColumn(file);
			GenericProcess(file);
			GenericClose(file);
		}

		//Apply complete cycle of basic processing (open-close)
		static void ProcessBasicFile(string fileName)
		{
			CSVFile file = GenericOpen(fileName);
			GenericProcess(file);
			GenericClose(file);
		}

		//Process the location CSVFile and then use it to generate a new file for NPCSpawns
		static void GenerateNPCSpawnFile()
		{
			CSVFile locationFile = GenericOpen("SeventySix_Location.csv");
			GenericProcess(locationFile);

			CSVFile npcSpawns = NpcSpawnHelper.ProcessNPCSpawns(locationFile, NpcSpawnHelper.SumLocationSpawnChances(locationFile));
			GenericProcess(npcSpawns);
			GenericClose(npcSpawns);
		}

		//Process the Junk Scrap and Component Quantity CSVFiles and then use them to generate a new file for Quantified Junk Scrap
		static void GenerateQuantifiedJunkScrapFile()
		{
			CSVFile componentQuantityFile = GenericOpen("SeventySix_Component_Quantity.csv");
			GenericProcess(componentQuantityFile);

			CSVFile junkScrapFile = GenericOpen("SeventySix_Junk_Scrap.csv");
			GenericProcess(junkScrapFile);

			CSVFile quantifiedJunkScrap = JunkScrap.ProcessJunkScrap(junkScrapFile, componentQuantityFile);
			GenericProcess(quantifiedJunkScrap);
			GenericClose(quantifiedJunkScrap);
		}

		//Shorthand to instantiate a new CSVFile from file name
		static CSVFile GenericOpen(string fileName)
		{
			return new CSVFile(inputPath + fileName);
		}

		//Apply the standard processing to the CSVFile
		static void GenericProcess(CSVFile file)
		{
			file.Sanitize();
			file.ReduceReferences();
			file.ReduceDecimals();
		}

		//Validate the file, write it to disk and free the memory.
		static void GenericClose(CSVFile file)
		{
			file.Validate();
			Directory.CreateDirectory(outputPath);
			file.WriteToFile(outputPath + "//");
			Console.WriteLine(file.fileName + " done.");
			GC.Collect();
		}
	}
}