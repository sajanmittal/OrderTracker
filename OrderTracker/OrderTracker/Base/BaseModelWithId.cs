namespace OrderTracker
{
	public abstract class BaseModelWithId : IBaseModel
	{
		public abstract int Id { get; }
	}
}