using OrderTracker.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OrderTracker
{
    public class MainViewModel : BaseViewModel<SearchItem>
    {
        public MainViewModel(INavigation navigation, Page page) : base(navigation, page)
        {
            SearchCommand = new Command(async () => await SearchAsync());
            AddOrderCommand = new Command(async () => await AddOrder());
            BackupCommand = new Command(async () => await BackupData());
            RestoreCommand = new Command(async () => await RestoreData());
        }

        public ICommand SearchCommand { get; private set; }

        private async Task SearchAsync()
        {
            await RunAsync(async () => { await Navigation.PushAsync(new OrderList(Model)); });
        }

        public ICommand AddOrderCommand { get; private set; }

        public async Task AddOrder()
        {
            await RunAsync(async () =>
            {
                await Navigation.PushAsync(new AddOrder());
            });
        }

        public ICommand BackupCommand { get; private set; }

        private async Task BackupData()
        {
            BackupRestoreStatus status = BackupRestoreStatus.Processing;

           await RunAsync(async () => {

               var backupDataCheck = await Page.DisplayAlert("Are you sure you want to backup data?", "Click yes to confirm", "Yes, I am Sure", "Cancel");

               if (backupDataCheck)
               {
                   var password = await Page.DisplayPromptAsync("Backup Data Password", "Please enter password to restore data", "Submit", "Cancel", "Password", 8, Keyboard.Text);
                   if (password != Constants.RESTORE_DATA_PASSWORD)
                       throw new Exception("Incorrect Password");

                   status = await BackupService.BackupData();
               }
            },
            (ex) => {
                status = BackupRestoreStatus.Failed;
            });

            switch(status)
            {
                case BackupRestoreStatus.Sucess:
                    LoggerService.LogInformation("Backup Success");
                    break;
                case BackupRestoreStatus.Failed:
                    LoggerService.LogInformation("Backup Failed");
                    break;
            }
        }

        public ICommand RestoreCommand { get; private set; }

        private async Task RestoreData()
        {
            BackupRestoreStatus status = BackupRestoreStatus.Processing;

            await RunAsync(async () =>
            {
                var setting =await SettingService.GetSettingValue<bool>(Constants.RESTORE_SETTING_KEY);
                if (setting)
                    throw new Exception("Data already restored!");

                var restoreDataCheck =await Page.DisplayAlert("Are you sure you want to restore data?", "Quation: Restoring wrongly may cause duplicate entries", "Yes, I am Sure", "Cancel");

                if (restoreDataCheck)
                {
                    var password = await Page.DisplayPromptAsync("Restore Data Password", "Please enter password to restore data", "Submit", "Cancel", "Password", 8, Keyboard.Text);
                    if (password != Constants.RESTORE_DATA_PASSWORD)
                        throw new Exception("Incorrect Password");

                    status = await BackupService.RestoreData();
                }
            },
            (ex) => {
                status = BackupRestoreStatus.Failed;
            });

            switch (status)
            {
                case BackupRestoreStatus.Sucess:
                    LoggerService.LogInformation("Data Restored Successfully");
                    break;
                case BackupRestoreStatus.Failed:
                    LoggerService.LogInformation("Restore Failed");
                    break;
                case BackupRestoreStatus.Processing:
                    break;
            }

        }
    }
}
