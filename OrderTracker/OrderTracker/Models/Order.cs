using SQLite;

namespace OrderTracker
{
    public class Order : BaseModelWithId
    {
        [Ignore]
        public override int Id => OrderId;

        [PrimaryKey]
        public int OrderId { get; set; }

        [Indexed(Unique = true)]
        public string TrackingNo { get; set; }

        [Indexed(Unique = false)]
        public string PhoneNo { get; set; }

        public string Detail { get; set; }

        public OrderStatus Status { get; set; }
    }
}
