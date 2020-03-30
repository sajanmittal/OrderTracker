using Newtonsoft.Json;
using SQLite;
using System;

namespace OrderTracker
{
    public class Order : BaseModelWithId
    {
        public Order()
        {
            OrderDate = DateTime.Now;
            Detail = string.Empty;
        }

        [Ignore]
        [JsonIgnore]
        public override int Id => OrderId;

        [PrimaryKey, AutoIncrement]
        [JsonIgnore]
        public int OrderId { get; set; }

        public string TrackingNo { get; set; }

        public string PhoneNo { get; set; }

        public string Detail { get; set; }

        public DateTime OrderDate { get; set; }

        public Enums.OrderStatus Status { get; set; }

        public string CloneNo { get; set; }
        [Ignore]
        [JsonIgnore]
        public string ShortDetail => Detail.Length <= 15 ? Detail : string.Concat(Detail.Substring(0, 15), "...");

    }
}
