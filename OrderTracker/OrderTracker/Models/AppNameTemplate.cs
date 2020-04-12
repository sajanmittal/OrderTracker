using System.Collections.Generic;

namespace OrderTracker
{
	public class AppNameTemplate
	{
		public AppName Item1 { get; set; }
		public AppName Item2 { get; set; }
		public AppName Item3 { get; set; }

		public bool IsSelected
		{
			get => (Item1 != null && Item1.IsSelected) || (Item2 != null && Item2.IsSelected) || (Item3 != null && Item3.IsSelected);
			set
			{
				if (Item1 != null)
				{
					Item1.IsSelected = value;
				}
				if (Item2 != null)
				{
					Item2.IsSelected = value;
				}
				if (Item3 != null)
				{
					Item3.IsSelected = value;
				}
			}
		}

		public AppNameTemplate(AppName[] apps)
		{
			if (apps.Length > 0)
			{
				Item1 = apps[0];
			}
			if (apps.Length > 1)
			{
				Item2 = apps[1];
			}
			if (apps.Length > 2)
			{
				Item3 = apps[2];
			}
		}

		public List<AppName> ToAppName()
		{
			List<AppName> apps = new List<AppName>();
			if (Item1 != null)
			{
				apps.Add(Item1);
			}
			if (Item2 != null)
			{
				apps.Add(Item2);
			}
			if (Item3 != null)
			{
				apps.Add(Item3);
			}

			return apps;
		}
	}
}