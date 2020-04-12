using System;
using System.Windows.Input;

namespace OrderTracker
{
	public interface IPassApplication
	{
		string Name { get; set; }

		Type PageType { get; }

		string Image { get; set; }

		ICommand Command { get; set; }
	}
}