using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace Mappalachia
{
	//Direct SQL queries and their execution
	static class Queries
	{
		static SqliteConnection connection;

		//Instantiate the connection to the database
		public static void CreateConnection()
		{
			connection = IOManager.OpenDatabase();
		}

		//Exceute a query to get all different signatures present
		public static SqliteDataReader ExecuteQuerySignatures()
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getSignatures;
			return query.ExecuteReader();
		}

		//Exceute a query to get all different signatures present
		public static SqliteDataReader ExecuteQueryLockLevels()
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getLockLevels;
			return query.ExecuteReader();
		}

		//Exceute a query to get all different variable NPC Spawns
		public static SqliteDataReader ExecuteQueryNPCTypes()
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getNPCTypes;
			return query.ExecuteReader();
		}

		//Exceute a query to get all different Scrap types
		public static SqliteDataReader ExecuteQueryScrapTypes()
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getScrapTypes;
			return query.ExecuteReader();
		}

		//Exceute a query to get all the cells from Cell table
		public static SqliteDataReader ExecuteQueryCells()
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getCells;
			return query.ExecuteReader();
		}

		//Executes just like a simple search but constrained to a given cellFormID
		public static SqliteDataReader ExecuteQuerySearchCell(string cellFormID, string searchTerm, List<string> filteredSignatures, List<string> filteredLockTypes)
		{
			searchTerm = DataHelper.ProcessSearchString(searchTerm);
			string queryString = Properties.Resources.searchCell;
			SqliteCommand query = connection.CreateCommand();

			//SQlite doesn't seem to support using variable length lists as parameters, but we can directly edit the query instead.
			queryString = queryString.Replace("$allowedSignatures", string.Join(",", filteredSignatures.Select(s => '\'' + s + '\'')));
			queryString = queryString.Replace("$allowedLockTypes", string.Join(",", filteredLockTypes.Select(s => '\'' + s + '\'')));

			query.CommandText = queryString;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$searchTerm", "%" + searchTerm + "%");
			query.Parameters.AddWithValue("$cellFormID", cellFormID);

			return query.ExecuteReader();
		}

		//Exceute the basic search query to search the worldspace and/or interiors
		public static SqliteDataReader ExecuteQueryStandardSearch(bool searchInterior, string searchTerm, List<string> filteredSignatures, List<string> filteredLockTypes)
		{
			searchTerm = DataHelper.ProcessSearchString(searchTerm);
			string queryString = searchInterior ? Properties.Resources.searchStandardAll : Properties.Resources.searchStandardAppalachia;
			SqliteCommand query = connection.CreateCommand();

			//SQlite doesn't seem to support using variable length lists as parameters, but we can directly edit the query instead.
			queryString = queryString.Replace("$allowedSignatures", string.Join(",", filteredSignatures.Select(s => '\'' + s + '\'')));
			queryString = queryString.Replace("$allowedLockTypes", string.Join(",", filteredLockTypes.Select(s => '\'' + s + '\'')));

			query.CommandText = queryString;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$searchTerm", "%" + searchTerm + "%");

			return query.ExecuteReader();
		}

		//Exceute a query to search for variable NPC Spawns
		public static SqliteDataReader ExecuteQueryNPCSearch(string searchTerm, double minChance, bool searchInterior)
		{
			SqliteCommand query = connection.CreateCommand();

			query.CommandText = searchInterior ? Properties.Resources.searchNPCAll : Properties.Resources.searchNPCAppalachia;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$searchTerm", searchTerm);
			query.Parameters.AddWithValue("$minChance", minChance);

			return query.ExecuteReader();
		}

		//Exceute a query to search for total scrap per cell/location
		public static SqliteDataReader ExecuteQueryScrapSearch(string searchTerm, bool searchInterior)
		{
			SqliteCommand query = connection.CreateCommand();

			query.CommandText = searchInterior ? Properties.Resources.searchScrapAll : Properties.Resources.searchScrapAppalachia;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$searchTerm", searchTerm);

			return query.ExecuteReader();
		}

		//Execute a query to find every coordinate of everything within a given cellFormID
		public static SqliteDataReader ExecuteQueryFindAllCoordinatesCell(string cellFormID)
		{
			SqliteCommand query = connection.CreateCommand();

			string queryString = Properties.Resources.getAllCoordsCell;

			query.CommandText = queryString;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$cellFormID", cellFormID);

			return query.ExecuteReader();
		}

		//Execute a query to find the coordinates of every instance of a given MapItem within an interior of a given cellFormID
		public static SqliteDataReader ExecuteQueryFindCoordinatesCell(string formID, string cellFormID, List<string> filteredLockTypes)
		{
			SqliteCommand query = connection.CreateCommand();

			string queryString = Properties.Resources.getCoordsCell;

			//SQlite doesn't seem to support using variable length lists as parameters, but we can directly edit the query instead.
			queryString = queryString.Replace("$allowedLockTypes", string.Join(",", filteredLockTypes.Select(s => '\'' + s + '\'')));

			query.CommandText = queryString;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$formID", formID);
			query.Parameters.AddWithValue("$cellFormID", cellFormID);

			return query.ExecuteReader();
		}

		//Execute a query to find the coordinates of every instance of a given MapItem
		public static SqliteDataReader ExecuteQueryFindCoordinatesStandard(string formID, List<string> filteredLockTypes)
		{
			SqliteCommand query = connection.CreateCommand();

			string queryString = Properties.Resources.getCoordsStandard;

			//SQlite doesn't seem to support using variable length lists as parameters, but we can directly edit the query instead.
			queryString = queryString.Replace("$allowedLockTypes", string.Join(",", filteredLockTypes.Select(s => '\'' + s + '\'')));

			query.CommandText = queryString;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$formID", formID);

			return query.ExecuteReader();
		}

		//Exceute a query to return the coordinates of NPC spawns given the NPC name and a minimum spawn chance
		public static SqliteDataReader ExecuteQueryFindCoordinatesNPC(string npc, double minChance)
		{
			SqliteCommand query = connection.CreateCommand();

			query.CommandText = Properties.Resources.getCoordsNPC;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$npc", npc);
			query.Parameters.AddWithValue("$minChance", minChance / 100.00);

			return query.ExecuteReader();
		}

		//Exceute a query to return the coordinates of the given scrap name within Junk items
		public static SqliteDataReader ExecuteQueryFindCoordinatesJunkScrap(string scrap)
		{
			SqliteCommand query = connection.CreateCommand();

			query.CommandText = Properties.Resources.getCoordsScrap;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$scrap", scrap);

			return query.ExecuteReader();
		}
	}
}
