using Xamarin.Forms;

namespace OrderTracker
{
	public class ToastHelper
	{
		private static IToastService toast;

		static ToastHelper()
		{
			if (toast == null)
				toast = DependencyService.Get<IToastService>();
		}

		public static void ToastMsg(string message, bool showInSnackBar = false)
		{
			if (showInSnackBar)
				toast.ShowSnackbar(message);
			else
				toast.ShowToast(message);
		}
	}
}