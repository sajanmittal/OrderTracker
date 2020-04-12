using Newtonsoft.Json;
using SQLite;

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

		[Ignore]
		public bool IsSelected { get; set; }
	}
}