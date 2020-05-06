using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Widget;
using OrderTracker.Droid.Renderers;
using OrderTracker.Views.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
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
			SetTintColor();
			if (!string.IsNullOrWhiteSpace(element.Title))
			{
				UpdateTitle(element.Title);
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			SetTintColor();
			
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
				EditText.SetHintTextColor(Color.Accent.ToAndroid());
			}
		}

		private void SetTintColor()
		{
			if (EditText.BackgroundTintList.DefaultColor != ColorStateList.ValueOf(Color.Accent.ToAndroid()).DefaultColor)
			{ var colorstate = ColorStateList.ValueOf(Color.Accent.ToAndroid());
				EditText.BackgroundTintList = colorstate;
				EditText.ForegroundTintList = colorstate;
				EditText.CompoundDrawableTintList = colorstate;
			}
		}
	}
}