using System;

namespace OrderTracker
{
	public class UtilityService
	{
		public static T ConvertTo<T>(object input, T defaultValue)
		{
			var result = defaultValue;
			try
			{
				if (input == null || input == DBNull.Value) return result;
				if (typeof(T).IsEnum)
				{
					result = (T)Enum.ToObject(typeof(T), ConvertTo(input, Convert.ToInt32(defaultValue)));
				}
				else
				{
					result = (T)Convert.ChangeType(input, typeof(T));
				}
			}
			catch (Exception ex)
			{
				LoggerService.LogError(ex);
			}

			return result;
		}
	}
}