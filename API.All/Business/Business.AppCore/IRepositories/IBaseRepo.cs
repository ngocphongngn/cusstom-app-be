using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Business.AppCore.IRepositories
{
    public interface IBaseRepo
    {
        IDatabaseProvider Provider { get; }
        void setReturnNewIdWhenInsert(bool value);
        List<T> Get<T>(string field, object value, string op = "=");
        List<T> Get<T>(IDbConnection cnn, string field, object value, string op = "=", string columns = "*");
        List<T> Get<T>(IDbTransaction transaction, string field, object value, string op = "=");
        List<T> Get<T>();
        object Insert(object entity);
        object Insert(IDbConnection cnn, object entity);
        object Insert(IDbTransaction transaction, object entity);
        bool Update(object entity, string fields = null);
        bool Delete(object entity);

    }
}