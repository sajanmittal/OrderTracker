using SQLite;

namespace OrderTracker
{
    public class Setting : BaseModelWithId
    {
        [Ignore]
        public override int Id => SettingId;

        [PrimaryKey, AutoIncrement]
        public int SettingId { get; set; }

        [Unique(Unique =true)]
        public string SettingName { get; set; }

        public string SettingValue { get; set; }
    }
}
