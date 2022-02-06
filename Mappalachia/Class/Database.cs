﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace Mappalachia
{
	// Direct SQL queries and their execution
	static class Database
	{
		static SqliteConnection connection;

		// Instantiate the connection to the database
		static Database()
		{
			connection = IOManager.OpenDatabase();
		}

		// Execute a query to get all different signatures present
		public static SqliteDataReader ExecuteQuerySignatures()
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getSignatures;
			return query.ExecuteReader();
		}

		// Execute a query to get all different signatures present
		public static SqliteDataReader ExecuteQueryLockLevels()
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getLockLevels;
			return query.ExecuteReader();
		}

		// Execute a query to get all different variable NPC Spawns
		public static SqliteDataReader ExecuteQueryNPCTypes()
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getNPCTypes;
			return query.ExecuteReader();
		}

		// Execute a query to get all different Scrap types
		public static SqliteDataReader ExecuteQueryScrapTypes()
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getScrapTypes;
			return query.ExecuteReader();
		}

		// Execute a query to get all the cells from Cell table
		public static SqliteDataReader ExecuteQueryCells()
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getSpaces;
			return query.ExecuteReader();
		}

		// Execute the basic search query to search the worldspace and/or interiors
		public static SqliteDataReader ExecuteQueryStandardSearch(string searchTerm, List<string> filteredSignatures, List<string> filteredLockTypes, string spaceFormID)
		{
			searchTerm = DataHelper.ProcessSearchString(searchTerm);
			string queryString = Properties.Resources.searchStandard;
			SqliteCommand query = connection.CreateCommand();

			// SQlite doesn't seem to support using variable length lists as parameters, but we can directly edit the query instead.
			queryString = queryString.Replace("$allowedSignatures", string.Join(",", filteredSignatures.Select(s => '\'' + s + '\'')));
			queryString = queryString.Replace("$allowedLockTypes", string.Join(",", filteredLockTypes.Select(s => '\'' + s + '\'')));

			query.CommandText = queryString;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$searchTerm", "%" + searchTerm + "%");
			query.Parameters.AddWithValue("$spaceFormID", spaceFormID);

			return query.ExecuteReader();
		}

		// Execute a query to search for variable NPC Spawns
		public static SqliteDataReader ExecuteQueryNPCSearch(string searchTerm, double minChance, string spaceFormID)
		{
			SqliteCommand query = connection.CreateCommand();

			query.CommandText =  Properties.Resources.searchNPC;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$searchTerm", searchTerm);
			query.Parameters.AddWithValue("$minChance", minChance);
			query.Parameters.AddWithValue("$spaceFormID", spaceFormID);

			return query.ExecuteReader();
		}

		// Execute a query to search for total scrap per cell/location
		public static SqliteDataReader ExecuteQueryScrapSearch(string searchTerm, string spaceFormID)
		{
			SqliteCommand query = connection.CreateCommand();

			query.CommandText = Properties.Resources.searchScrap;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$searchTerm", searchTerm);
			query.Parameters.AddWithValue("$spaceFormID", spaceFormID);

			return query.ExecuteReader();
		}

		// Execute a query to find every coordinate of everything within a given cellFormID
		public static SqliteDataReader ExecuteQueryFindAllCoordinatesCell(string cellFormID)
		{
			SqliteCommand query = connection.CreateCommand();

			string queryString = Properties.Resources.getAllCoordsSpace;

			query.CommandText = queryString;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$spaceFormID", cellFormID);

			return query.ExecuteReader();
		}

		// Execute a query to find the coordinates of every instance of a given MapItem
		public static SqliteDataReader ExecuteQueryFindCoordinatesStandard(string formID, string spaceFormID, List<string> filteredLockTypes)
		{
			SqliteCommand query = connection.CreateCommand();

			string queryString = Properties.Resources.getCoordsStandard;

			// SQlite doesn't seem to support using variable length lists as parameters, but we can directly edit the query instead.
			queryString = queryString.Replace("$allowedLockTypes", string.Join(",", filteredLockTypes.Select(s => '\'' + s + '\'')));

			query.CommandText = queryString;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$formID", formID);
			query.Parameters.AddWithValue("$spaceFormID", spaceFormID);

			return query.ExecuteReader();
		}

		// Execute a query to return the coordinates of NPC spawns given the NPC name and a minimum spawn chance
		public static SqliteDataReader ExecuteQueryFindCoordinatesNPC(string npc, string spaceFormID, double minChance)
		{
			SqliteCommand query = connection.CreateCommand();

			query.CommandText = Properties.Resources.getCoordsNPC;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$npc", npc);
			query.Parameters.AddWithValue("$minChance", minChance / 100.00);
			query.Parameters.AddWithValue("$spaceFormID", spaceFormID);

			return query.ExecuteReader();
		}

		// Execute a query to return the coordinates of the given scrap name within Junk items
		public static SqliteDataReader ExecuteQueryFindCoordinatesJunkScrap(string scrap, string spaceFormID)
		{
			SqliteCommand query = connection.CreateCommand();

			query.CommandText = Properties.Resources.getCoordsScrap;
			query.Parameters.Clear();
			query.Parameters.AddWithValue("$scrap", scrap);
			query.Parameters.AddWithValue("$spaceFormID", spaceFormID);

			return query.ExecuteReader();
		}

		// Execute a query to return the game version string
		public static SqliteDataReader ExecuteQueryGameVersion()
		{
			SqliteCommand query = connection.CreateCommand();
			query.CommandText = Properties.Resources.getGameVersion;
			return query.ExecuteReader();
		}
	}
}
