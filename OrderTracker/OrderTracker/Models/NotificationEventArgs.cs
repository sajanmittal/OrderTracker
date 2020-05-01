using System;

namespace OrderTracker
{
	public class NotificationEventArgs : EventArgs
	{
		public NotificationInfo Info { get; set; }
	}
}