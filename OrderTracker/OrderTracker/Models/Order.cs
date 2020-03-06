using SQLite;
using System;

namespace OrderTracker
{
    public class Order : BaseModelWithId
    {
        public Order()
        {
            OrderDate = DateTime.Now;
        }

        [Ignore]
        public override int Id => OrderId;

        [PrimaryKey]
        public int OrderId { get; set; }

        public string TrackingNo { get; set; }

        public string PhoneNo { get; set; }

        public string Detail { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        [Ignore]
        public string ShortDetail => Detail.Length <= 15 ? Detail : string.Concat(Detail.Substring(0, 15), "...");

    }
}
