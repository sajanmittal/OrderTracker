namespace OrderTracker
{
	public interface IToastService
	{
		void ShowToast(string message);

		void ShowSnackbar(string message);
	}
}