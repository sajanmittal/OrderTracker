using Newtonsoft.Json;
using SQLite;

namespace OrderTracker
{
	public class PhoneAppLink : BaseModelWithId
	{
		[PrimaryKey, AutoIncrement]
		public int PhoneAppLinkId { get; set; }

		public int PhoneInfoId { get; set; }

		public int AppNameId { get; set; }

		public string PhoneNo { get; set; }

		[Ignore]
		[JsonIgnore]
		public override int Id => PhoneAppLinkId;
	}
}
