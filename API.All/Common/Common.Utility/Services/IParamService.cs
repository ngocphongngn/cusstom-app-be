using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utility.Services
{
    public interface IParamService
    {
        string ParseSort(string sort);
        string ParseColumn(string column);
        string ParseColumn<T>(string column);
        //WhereParameter ParseFilter(string input, WhereParameter where = null);
        Dictionary<string, object> ParseClient(string jsonObject);
    }
}
