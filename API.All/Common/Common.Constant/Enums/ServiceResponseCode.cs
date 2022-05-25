using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Constant.Enums
{
    public enum ServiceResponseCode : int
    {
        Success = 0,
        InvalidData = 1,
        NotFound = 2,
        RequireConfirm = 3,
        NotAuthorize = 4,
        Arisened = 5,
        Forbiden = 403,
        Error = 99,
        Exception = 999,
        PartInvalidData = 6,
        OverQuantity = 7,
        ProcessAsync = 100,
    }
}
