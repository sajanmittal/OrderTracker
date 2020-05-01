using Xamarin.Forms;

namespace OrderTracker.Views.Controls
{
	public class PassDatePicker : DatePicker
	{
		public static readonly BindableProperty TitleProperty = BindableProperty.Create(propertyName: nameof(Title), returnType: typeof(string), declaringType: typeof(PassDatePicker), defaultValue: default(string), defaultBindingMode: BindingMode.TwoWay);

		public string Title
		{
			get => (string)GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}
	}
}