using System;
using System.Collections;
using Newtonsoft.Json;

namespace OrderTracker
{
	public class BackupModel
	{
		[JsonProperty("Table")]
		public Type TableType { get; set; }

		[JsonProperty("Data")]
		public IList BackupData { get; set; }
	}
}