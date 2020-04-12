using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrderTracker
{
	public class ToggleConverter : IValueConverter, IMarkupExtension
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Toggle(value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Toggle(value);
		}

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}

		private object Toggle(object value)
		{
			try
			{
				if (value is bool)
				{
					bool result = System.Convert.ToBoolean(value);
					return !result;
				}
			}
			catch (Exception ex)
			{
				LoggerService.LogError(ex);
			}

			return value;
		}
	}
}