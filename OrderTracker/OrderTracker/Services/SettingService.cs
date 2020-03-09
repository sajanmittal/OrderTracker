using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderTracker
{
    public class SettingService
    {
        public async static Task<List<Setting>> GetAllSettings()
        {
            return await App.DbService.SelectAsync<Setting>();
        }

        public async static Task<T> GetSettingValue<T>(string settingName) 
        {
            var setting = await App.DbService.FirstOrDefault<Setting>(x => x.SettingName == settingName);
            if (setting == null || string.IsNullOrEmpty(setting.SettingValue))
                return default;

            return UtilityService.ConvertTo<T>(setting.SettingValue, default);
        }

        public async static Task<Setting> GetSetting(string settingName)
        {
            return await App.DbService.FirstOrDefault<Setting>(x => x.SettingName == settingName);
        }

    }
}
