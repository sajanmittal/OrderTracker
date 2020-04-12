using System;
using System.Windows.Input;

namespace OrderTracker
{
	public class PassApplication : IPassApplication
	{
		public string Name { get; set; }

		public virtual Type PageType => null;

		public string Image { get; set; }

		public virtual ICommand Command { get; set; }
	}
}