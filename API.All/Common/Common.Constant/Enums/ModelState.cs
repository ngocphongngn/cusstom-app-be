using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Constant.Enums
{
    public enum ModelState : int
    {
        None = 0,
        Insert = 1,
        Update = 2,
        Delete = 3
    }
    public enum DeleteType: int
    {
        Single = 1,
        Multi = 2
    }
}
