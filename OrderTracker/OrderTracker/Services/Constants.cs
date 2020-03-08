using System;
using System.IO;
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
    }
}
