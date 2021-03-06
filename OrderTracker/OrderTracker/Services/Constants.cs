﻿using System.IO;
using Xamarin.Essentials;

namespace OrderTracker
{
	public static class Constants
	{
		public const string DatabaseFilename = "OTSQLite.db3";

		public const SQLite.SQLiteOpenFlags Flags =
			 // open the database in read/write mode
			 SQLite.SQLiteOpenFlags.ReadWrite |
			 // create the database if it doesn't exist
			 SQLite.SQLiteOpenFlags.Create |
			 // enable multi-threaded database access
			 SQLite.SQLiteOpenFlags.SharedCache;

		public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

		public const string SAVED_MSG = "Saved Successfully";

		public const string SAVE_OTHER_MSG = "Want to Save Another?";

		public const string UPDATE_TRACKING_NO = "Update Tracking Number";

		public const string BACKUP_FILE_NAME = "OrderTrackerBackup.json";

		public const string RESTORE_SETTING_KEY = "IsDataRestored";

		public const string RESTORE_DATA_PASSWORD = "abcd1234";

		public const string EXCEL_XTSN = ".xlsx";

		public const string FILE_DATE_FRMT = "yyyy-MM-dd HH-mm-ss";

		public const string DATE_FRMT = "dd-MMM-yyyy";

		public const int TASK_TIMEOUT = 180;

		public const string CHANNEL_ID = "pass_default";

		public const string CHANNEL_NAME = "Pass Default";

		public const string TITLE_KEY = "title";

		public const string MESSAGE_KEY = "message";
	}
}