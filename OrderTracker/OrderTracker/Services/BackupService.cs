using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace OrderTracker
{
	public class BackupService
	{
		public async static Task<Enums.BackupRestoreStatus> BackupData(List<IBackupModule> backups)
		{
			if (backups.Any())
			{
				List<BackupModel> tableBackups = new List<BackupModel>();
				foreach (var backup in backups)
				{
					var backupData = new BackupModel { TableType = backup.BackupTableType, BackupData = await backup.GetBckupData() };
					tableBackups.Add(backupData);
				}

				var serializedData = JsonConvert.SerializeObject(tableBackups);
				var fileName = Path.Combine(DependencyService.Get<IFileAccessService>().GetFileLocation(), Constants.BACKUP_FILE_NAME);
				File.WriteAllText(fileName, serializedData);
			}
			return Enums.BackupRestoreStatus.Sucess;
		}

		public async static Task<Enums.BackupRestoreStatus> RestoreData()
		{
			var fileName = Path.Combine(DependencyService.Get<IFileAccessService>().GetFileLocation(), Constants.BACKUP_FILE_NAME);
			var restoreSetting = await SettingService.GetSettingValue<bool>(Constants.RESTORE_SETTING_KEY);
			if (!restoreSetting && File.Exists(fileName))
			{
				var data = File.ReadAllText(fileName);
				if (data.Length > 0)
				{
					var tableBackups = JsonConvert.DeserializeObject<List<BackupModel>>(data);
					if (tableBackups.Any())
					{
						foreach (var backupData in tableBackups)
						{
							var isInserted = await App.DbService.BulkInsertAsync(backupData.TableType, backupData.BackupData);
							if (isInserted > 0)
							{
								var setting = await SettingService.GetSetting(Constants.RESTORE_SETTING_KEY);
								if (setting != null)
								{
									setting.SettingValue = "true";
									await App.DbService.UpdateAsync(setting);
									return Enums.BackupRestoreStatus.Sucess;
								}
							}
						}
					}
				}
			}

			return Enums.BackupRestoreStatus.Failed;
		}
	}
}