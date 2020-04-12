using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrderTracker
{
	public class NullValueBoolConverter : IValueConverter, IMarkupExtension
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string)
			{
				string val = value as string;
				return !string.IsNullOrWhiteSpace(val);
			}

			return !(value == null);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}
}