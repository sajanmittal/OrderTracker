using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Xamarin.Forms;
using static Android.Support.V4.App.NotificationCompat;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(OrderTracker.Droid.NotificationService))]

namespace OrderTracker.Droid
{
	public class NotificationService : INotificationService
	{
		private bool channelInitialized = false;
		private int messageId = 100;
		private NotificationManager manager;

		public event EventHandler NotificationReceived;

		public void Initialize()
		{
			CreateNotificationChannel();
		}

		public void ReceiveNotification(NotificationInfo info)
		{
			var args = new NotificationEventArgs()
			{
				Info = info
			};
			NotificationReceived?.Invoke(null, args);
		}

		public int ScheduleNotification(NotificationInfo info)
		{
			if (!channelInitialized)
			{
				CreateNotificationChannel();
			}

			Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
			intent.AddFlags(ActivityFlags.ClearTop);
			intent.PutExtra(Constants.TITLE_KEY, info.Title);
			intent.PutExtra(Constants.MESSAGE_KEY, info.Message);

			PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, 0, intent, PendingIntentFlags.OneShot);

			Builder builder = new Builder(AndroidApp.Context, Constants.CHANNEL_ID)
				.SetContentIntent(pendingIntent)
				.SetContentTitle(info.Title)
				.SetContentText(info.Message)
				.SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.track))
				.SetSmallIcon(Resource.Drawable.track)
				.SetDefaults((int)NotificationDefaults.All)
				.SetPriority(PriorityHigh)
				.SetStyle(new BigTextStyle())
				.SetColor(Resource.Color.colorAccent)
				.SetTimeoutAfter(TimeSpan.FromMinutes(30).Milliseconds)
				.SetAutoCancel(true);

			Notification notification = builder.Build();
			manager.Notify(messageId, notification);

			return messageId;
		}

		private void CreateNotificationChannel()
		{
			manager = AndroidApp.Context.GetSystemService(Context.NotificationService).JavaCast<NotificationManager>();

			if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
			{
				var channelNameJava = new Java.Lang.String(Constants.CHANNEL_NAME);
				var channel = new NotificationChannel(Constants.CHANNEL_ID, channelNameJava, NotificationImportance.High)
				{
					Description = "PASS Default notification channel"
				};
				manager.CreateNotificationChannel(channel);
			}

			channelInitialized = true;
		}
	}
}