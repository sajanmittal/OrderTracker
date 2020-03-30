using System;
using System.Collections.Generic;
using System.Text;

namespace OrderTracker
{
    public abstract class BaseModelWithId : IBaseModel
    {
        public abstract int Id { get; }
    }
}
