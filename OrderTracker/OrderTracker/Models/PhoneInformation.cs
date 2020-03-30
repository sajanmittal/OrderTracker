using Newtonsoft.Json;
using SQLite;
using System;

namespace OrderTracker
{
	public class PhoneInformation : BaseModelWithId
	{
		public PhoneInformation()
		{
			PhoneNo = string.Empty;
			SimNo = string.Empty;
			Company = string.Empty;
			ExpiryDate = DateTime.Today;
		}

		[PrimaryKey, AutoIncrement]
		public int PhoneInfoId { get; set; }

		[Ignore]
		[JsonIgnore]
		public override int Id => PhoneInfoId;

		[Unique]
		public string PhoneNo { get; set; }

		public string SimNo { get; set; }

		public string Company { get; set; }

		public DateTime ExpiryDate { get; set; }
	}
}
