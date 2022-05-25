using Common.Constant.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.Base
{
    public interface IRecordState
    {
        ModelState EntityState { get; set; }
    }
}
