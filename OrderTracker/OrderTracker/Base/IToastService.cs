using System;
using System.Collections.Generic;
using System.Text;

namespace OrderTracker
{
    public interface IToastService
    {
        void ShowToast(string message);
        void ShowSnackbar(string message);
    }
}
