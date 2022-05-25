using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utility.Services
{
    public class ParamService : IParamService
    {
        private readonly ITypeService _typeService;
        public ParamService(ITypeService typeService)
        {
            _typeService = typeService;
        }
        public Dictionary<string, object> ParseClient(string jsonObject)
        {
            throw new NotImplementedException();
        }

        public string ParseColumn(string column)
        {
            throw new NotImplementedException();
        }

        public string ParseColumn<T>(string column)
        {
            throw new NotImplementedException();
        }

        public string ParseSort(string sort)
        {
            throw new NotImplementedException();
        }
    }
}
