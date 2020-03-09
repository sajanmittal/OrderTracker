using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;

namespace OrderTracker
{
    public class BackupService
    {
        public async static Task<BackupRestoreStatus> BackupData()
        {
            List<Order> orderData = await App.DbService.SelectAsync<Order>();

            if(orderData.Any())
            {
                var serializedData = JsonConvert.SerializeObject(orderData);
                var fileName = Path.Combine(DependencyService.Get<IFileAccessService>().GetFileLocation(), Constants.BACKUP_FILE_NAME);
                File.WriteAllText(fileName, serializedData);
            }
            return BackupRestoreStatus.Sucess;
        }

        public async static Task<BackupRestoreStatus> RestoreData()
        {
            var fileName = Path.Combine(DependencyService.Get<IFileAccessService>().GetFileLocation(), Constants.BACKUP_FILE_NAME);
            var restoreSetting = await SettingService.GetSettingValue<bool>(Constants.RESTORE_SETTING_KEY);
            if (!restoreSetting && File.Exists(fileName))
            {
                var data = File.ReadAllText(fileName);
                if(data.Length > 0)
                {
                    var backupData = JsonConvert.DeserializeObject<List<Order>>(data);
                    if(backupData.Any())
                    {
                        var isInserted = await App.DbService.BulkInsertAsync(backupData);
                        if(isInserted > 0)
                        {
                            var setting = await SettingService.GetSetting(Constants.RESTORE_SETTING_KEY);
                            if(setting != null)
                            {
                                setting.SettingValue = "true";
                                int isUpdated = await App.DbService.UpdateAsync(setting);
                                return BackupRestoreStatus.Sucess;
                            }
                        }
                    }
                }
            }

            return BackupRestoreStatus.Failed;
        }
    }
}
