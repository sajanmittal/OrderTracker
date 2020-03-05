using System;
using System.Collections.Generic;
using System.Text;

namespace OrderTracker
{
    public class LoggerService
    {
        public static void LogError(Exception ex)
        {
            var exception = FindInnerException(ex);
            ToastHelper.ToastMsg(ex.Message, true);
        }

        public static void LogInformation(string message)
        {
            ToastHelper.ToastMsg(message);
        }

        private static Exception FindInnerException(Exception ex)
        {
            if (ex.InnerException != null)
                FindInnerException(ex.InnerException);

            return ex;
        }
    }
}
