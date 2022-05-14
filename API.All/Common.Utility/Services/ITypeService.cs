using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Common.Utility.Services
{
    public interface ITypeService
    {
        IList CreateList(Type type);
        bool IsList(Type type);
    }
}
