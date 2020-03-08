using Xamarin.Forms;

[assembly: Dependency(typeof(OrderTracker.Droid.FileAccessService))]
namespace OrderTracker.Droid
{
    public class FileAccessService : IFileAccessService
    {
        public string GetFileLocation()
        {
            return Android.OS.Environment.DirectoryDownloads;
        }
    }
}