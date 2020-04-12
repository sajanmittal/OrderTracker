﻿using Android.Support.Design.Widget;
using Android.Widget;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(OrderTracker.Droid.ToastService))]

namespace OrderTracker.Droid
{
	public class ToastService : IToastService
	{
		public void ShowSnackbar(string message)
		{
			var activity = CrossCurrentActivity.Current.Activity;
			Android.Views.View activityRootView = activity.FindViewById(Android.Resource.Id.Content);
			Snackbar.Make(activityRootView, message, Snackbar.LengthLong).Show();
		}

		public void ShowToast(string message)
		{
			Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
		}
	}
}