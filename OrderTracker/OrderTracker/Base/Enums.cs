using System;

namespace OrderTracker
{
	public static class Enums
	{
		public enum BackupRestoreStatus
		{
			Sucess,
			Failed,
			Processing
		}

		public enum OrderStatus
		{
			Received = 0,
			Cancelled = 100,
			Pending = 200
		}
	}

	public static class Flags
	{
		[Flags]
		public enum AppStatupFlags
		{
			None,
			Notification,
		}
	}
}