using System;
using System.Collections.Generic;
using System.Text;

namespace OrderTracker
{
    public abstract class BaseModelWithId : BaseModel
    {
        public abstract int Id { get; }
    }
}
