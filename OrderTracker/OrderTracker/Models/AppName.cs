using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderTracker
{
	public class AppName : BaseModelWithId
	{
		public AppName()
		{
			Name = string.Empty;
		}

		[PrimaryKey, AutoIncrement]
		public int AppNameId { get; set; }

		[Unique]
		public string Name { get; set; }

		[Ignore]
		[JsonIgnore]
		public override int Id => AppNameId;
	}
}
