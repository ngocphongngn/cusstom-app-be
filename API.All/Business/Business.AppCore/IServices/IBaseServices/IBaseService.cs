using System;
using System.Collections.Generic;
using System.Text;

namespace Business.AppCore.IServices.IBaseServices
{
    public interface IBaseService
    {
        object Insert(object entity);
        bool Update(object entity, string fields = null);
        void Delete(object entity);
        List<T> Get<T>(string field, object value, string op = "=");
        string GetConnectionString();
    }
}
