using System;
using System.Collections;
using System.Threading.Tasks;

namespace OrderTracker
{
	public class BackupModule<T> : IBackupModule where T : IBaseModel, new()
	{
		public Type BackupTableType => typeof(T);

		public async Task<IList> GetBckupData()
		{
			return await App.DbService.SelectAsync<T>();
		}
	}
}