using System;

namespace OrderTracker
{
	public interface INotificationService
	{
		event EventHandler NotificationReceived;

		void Initialize();

		int ScheduleNotification(NotificationInfo info);

		void ReceiveNotification(NotificationInfo info);
	}
}