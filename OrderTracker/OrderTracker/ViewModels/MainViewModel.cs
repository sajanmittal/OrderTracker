using OrderTracker.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OrderTracker
{
   public class MainViewModel : ViewModelBase<EmptyModel>
   {
      public MainViewModel(Page page) : base(page)
      {
         BackupCommand = new Command(async () => await BackupData());
         RestoreCommand = new Command(async () => await RestoreData());
      }

      public ICommand BackupCommand { get; private set; }

      private async Task BackupData()
      {
         Enums.BackupRestoreStatus status = Enums.BackupRestoreStatus.Processing;

         await RunAsync(async () => {

            var backupDataCheck = await BindingPage.DisplayAlert("Are you sure you want to backup data?", "Click yes to confirm", "Yes, I am Sure", "Cancel");

            if (backupDataCheck)
            {
               List<IBackupModule> backupModules = new List<IBackupModule>() {
                  new BackupModule<Order>(),
                  new BackupModule<PhoneInformation>(),
                  new BackupModule<PhoneAppLink>(),
                  new BackupModule<AppName>(),
                  new BackupModule<Setting>(),
                 };

               status = await BackupService.BackupData(backupModules);
            }
         },
          (ex) => {
             status = Enums.BackupRestoreStatus.Failed;
          });

         switch (status)
         {
            case Enums.BackupRestoreStatus.Sucess:
               LoggerService.LogInformation("Backup Success");
               break;
            case Enums.BackupRestoreStatus.Failed:
               LoggerService.LogInformation("Backup Failed");
               break;
         }
      }

      public ICommand RestoreCommand { get; private set; }

      private async Task RestoreData()
      {
         Enums.BackupRestoreStatus status = Enums.BackupRestoreStatus.Processing;

         await RunAsync(async () =>
         {
            var setting = await SettingService.GetSettingValue<bool>(Constants.RESTORE_SETTING_KEY);
            if (setting)
               throw new Exception("Data already restored!");

            var restoreDataCheck = await BindingPage.DisplayAlert("Are you sure you want to restore data?", "Quation: Restoring wrongly may cause duplicate entries", "Yes, I am Sure", "Cancel");

            if (restoreDataCheck)
            {
               status = await BackupService.RestoreData();
            }
         },
         (ex) => {
            status = Enums.BackupRestoreStatus.Failed;
         });

         switch (status)
         {
            case Enums.BackupRestoreStatus.Sucess:
               LoggerService.LogInformation("Data Restored Successfully");
               break;
            case Enums.BackupRestoreStatus.Failed:
               LoggerService.LogInformation("Restore Failed");
               break;
            case Enums.BackupRestoreStatus.Processing:
               break;
         }

      }


      public List<IPassApplication> GetApplications()
      {
         return new List<IPassApplication>
         {
            new PassPageApplication<AddOrder>(this){ Name= "Order Tracker", Image = "order_tracker"},
            new PassPageApplication<AddPhoneInfo>(this){Name="Phone Info", Image="sim_icon"},
            new PassPageApplication<AddApp>(this){Name="App Info", Image="app_info"},
            new PassApplication{Name = "Backup Data", Image = "backup", Command = BackupCommand}
         };
      }

    }
}
