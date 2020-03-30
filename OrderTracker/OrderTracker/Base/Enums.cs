using System;
using System.Collections.Generic;
using System.Text;

namespace OrderTracker
{
	public static class Enums
	{
      public enum BackupRestoreStatus
      {
         Sucess,
         Failed,
         Processing
      }

      public enum OrderStatus
      {
         Received = 0,
         Cancelled = 100,
         Pending = 200
      }

   }
}
