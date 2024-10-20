using System.Diagnostics;
using Library;
using Microsoft.Data.Sqlite;

namespace BackgroundRenderer;

class BackgroundRenderer
{
	static void Main()
	{
		Console.Title = "Mappalachia Background Renderer";
		SqliteConnection connection = BuildTools.GetNewConnection();

		BuildTools.StdOutWithColor("Enter a space-separated list of space EditorIDs to render. Or enter nothing to render all", BuildTools.QuestionColor);
		string input = Console.ReadLine() ?? string.Empty;

		Stopwatch stopwatch = Stopwatch.StartNew();

		List<string> requestedIDs = input.Trim().Split(" ").Where(space => !string.IsNullOrWhiteSpace(space)).ToList();

		// Select spaces unconditionally if none provided, otherwise select only the list of provided spaces
		string query = $"SELECT * FROM Space{(requestedIDs.Count == 0 ? string.Empty : $" WHERE spaceEditorID IN {requestedIDs.ToSqliteCollection()}")};";
		List<Space> spaces = CommonDatabase.GetSpaces(connection, query);

		// If we specified cells, ensure we found them all in the DB before proceeding
		if (requestedIDs.Count != 0 && spaces.Count != requestedIDs.Count)
		{
			throw new Exception("Inputted cells were not all found. Check for typos in EditorIDs");
		}

		BuildTools.StdOutWithColor($"Rendering Finished. {stopwatch.Elapsed}", BuildTools.InfoColor);
		Console.ReadKey();
	}
}
