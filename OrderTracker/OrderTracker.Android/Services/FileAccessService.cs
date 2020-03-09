using Xamarin.Forms;
using Android.OS;

[assembly: Dependency(typeof(OrderTracker.Droid.FileAccessService))]
namespace OrderTracker.Droid
{
    public class FileAccessService : IFileAccessService
    {
        public string GetFileLocation()
        {
            return Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads).AbsolutePath;
        }
    }
}