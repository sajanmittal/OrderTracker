using Android.Content;
using Android.Widget;
using OrderTracker.Droid.Renderers;
using OrderTracker.Views.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;
using AColor = Android.Graphics.Color;
using XDatePicker = Xamarin.Forms.DatePicker;

[assembly: ExportRenderer(typeof(PassDatePicker), typeof(PassDatePickerRenderer), new[] { typeof(VisualMarker.MaterialVisual) })]

namespace OrderTracker.Droid.Renderers
{
	public class PassDatePickerRenderer : MaterialDatePickerRenderer
	{
		private MaterialPickerTextInputLayout _layout;

		public PassDatePickerRenderer(Context context) : base(context)
		{
		}

		protected override MaterialPickerTextInputLayout CreateNativeControl()
		{
			_layout = base.CreateNativeControl();
			return _layout;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<XDatePicker> e)
		{
			base.OnElementChanged(e);
			PassDatePicker element = Element as PassDatePicker;

			if (!string.IsNullOrWhiteSpace(element.Title))
			{
				UpdateTitle(element.Title);
			}
		}

		public void UpdateTitle(string hint)
		{
			SetHint(hint, Element);
		}

		private void SetHint(string hint, VisualElement element)
		{
			_layout.HintEnabled = !string.IsNullOrWhiteSpace(hint);
			if (_layout.HintEnabled)
			{
				element?.InvalidateMeasureNonVirtual(InvalidationTrigger.VerticalOptionsChanged);
				_layout.Hint = hint;
				EditText.SetHintTextColor(AColor.Transparent);
			}
		}
	}
}