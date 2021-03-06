﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderTracker.Views;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;

namespace OrderTracker
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	public partial class MainPage : ContentPage
	{
		public MainViewModel viewModel { get; set; }

		public MainPage(bool isRedirected = false)
		{
			InitializeComponent();
			if (viewModel == null)
			{
				viewModel = new MainViewModel(this);
				BindingContext = viewModel;
			}
			FireNotification(isRedirected);
		}

		private void FireNotification(bool isRedirected)
		{
			var notificationService = DependencyService.Get<INotificationService>();
			notificationService.NotificationReceived += async (o, e) =>
			{
				await viewModel.PushAsync(new SearchPhoneInfo(true));
			};
			if (!isRedirected)
			{
				Task.Run(async () =>
				{
					var expiryDate = DateTime.Now.AddDays(4);
					var expiringSims = await App.DbService.SelectAsync<PhoneInformation>(x => x.ExpiryDate <= expiryDate);
					if (expiringSims.Count > 0)
					{
						var info = new NotificationInfo
						{
							Title = $"{expiringSims.Count} Expiring SIMS!!",
							Message = $"{expiringSims.Count} SIMS are expiring before {expiryDate:dd-MMM-yyyy} by next 4 days."
						};
						notificationService.ScheduleNotification(info);
					}
				});
			}
		}

		protected override void OnAppearing()
		{
			AddAppTiles();
			base.OnAppearing();
		}

		private void AddAppTiles()
		{
			var apps = viewModel.GetApplications();
			absLayout.Children.Clear();
			int batchCount = 2;

			var grid = new Grid
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				ColumnDefinitions = GetColumns(batchCount)
			};
			AbsoluteLayout.SetLayoutBounds(grid, new Rectangle(0, 0, 1, 0.8));
			AbsoluteLayout.SetLayoutFlags(grid, AbsoluteLayoutFlags.All);

			int rowCount = 0;

			Dictionary<int, IPassApplication[]> appList = new Dictionary<int, IPassApplication[]>();

			for (int i = 0; i < apps.Count; i += batchCount)
			{
				var count = batchCount;
				if (i + batchCount > apps.Count - 1)
					count = apps.Count - i;

				var currentApps = apps.GetRange(i, count).ToArray();
				appList.Add(rowCount, currentApps);
				rowCount++;
			}

			grid.RowDefinitions = GetRows(rowCount);

			for (int i = 0; i < rowCount; i++)
			{
				int j = 0;
				foreach (var app in appList[i])
				{
					var layout = TileLayout(app);
					Grid.SetRow(layout, i);
					Grid.SetColumn(layout, j);
					grid.Children.Add(layout);
					j++;
				}
			}

			absLayout.Children.Add(grid);
		}

		private StackLayout TileLayout(IPassApplication app)
		{
			var layout = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				BackgroundColor = Color.Transparent
			};

			var button = new ImageButton
			{
				Command = app.Command,
				Source = $"{app.Image}.png",
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = 80,
				WidthRequest = 100,
				BorderColor = Color.Transparent,
				BackgroundColor = Color.Transparent,
				Aspect = Aspect.AspectFit
			};

			var label = new Label
			{
				FontSize = 18,
				FontAttributes = FontAttributes.Bold,
				Text = app.Name,
				VerticalOptions = LayoutOptions.EndAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

			layout.Children.Add(button);
			layout.Children.Add(label);

			return layout;
		}

		private RowDefinitionCollection GetRows(int count)
		{
			var rows = new RowDefinitionCollection();
			for (int i = 0; i < count; i++)
			{
				rows.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			}
			return rows;
		}

		private ColumnDefinitionCollection GetColumns(int count)
		{
			var columns = new ColumnDefinitionCollection();
			for (int i = 0; i < count; i++)
			{
				columns.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			}
			return columns;
		}
	}
}