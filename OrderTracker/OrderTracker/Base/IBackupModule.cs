using System;
using System.Collections;
using System.Threading.Tasks;

namespace OrderTracker
{
	public interface IBackupModule
	{
		Type BackupTableType { get; }

		Task<IList> GetBckupData();
	}
}
