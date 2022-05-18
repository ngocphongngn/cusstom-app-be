using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Common.Utility.Services
{
    public class TypeService : ITypeService
    {
        public IList CreateList(Type type)
        {
            Type listType = typeof(List<>).MakeGenericType(new[] { type });
            IList list = (IList)Activator.CreateInstance(listType);
            return list;
        }

        public bool IsList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }
    }
}
