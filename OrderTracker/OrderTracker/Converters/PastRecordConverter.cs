using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrderTracker.Converters
{
	public class PastRecordConverter : IValueConverter, IMarkupExtension
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DateTime date)
			{
				return date <= DateTime.Today;
			}
			else if (value is string val)
			{
				var isDate = DateTime.TryParse(val, out DateTime expiryDate);
				if (isDate)
					return expiryDate <= DateTime.Today;
			}
			return false;
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