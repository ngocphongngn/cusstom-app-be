using Business.AppCore.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Business.Infrastructure.BaseRepositories
{
    public class BaseRepo : IBaseRepo
    {
        protected readonly IDatabaseProvider _dbProvider;
        public BaseRepo(string connectionString)
        {
            this._dbProvider = this.CreateProvider(connectionString);
        }

        protected virtual IDatabaseProvider CreateProvider(string connectionString)
        {
            throw new NotImplementedException();
        }
        public IDatabaseProvider Provider => throw new NotImplementedException();

        public bool Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public List<T> Get<T>(string field, object value, string op = "=")
        {
            throw new NotImplementedException();
        }

        public List<T> Get<T>(IDbConnection cnn, string field, object value, string op = "=", string columns = "*")
        {
            throw new NotImplementedException();
        }

        public List<T> Get<T>(IDbTransaction transaction, string field, object value, string op = "=")
        {
            throw new NotImplementedException();
        }

        public List<T> Get<T>()
        {
            throw new NotImplementedException();
        }

        public object Insert(object entity)
        {
            throw new NotImplementedException();
        }

        public object Insert(IDbConnection cnn, object entity)
        {
            throw new NotImplementedException();
        }

        public object Insert(IDbTransaction transaction, object entity)
        {
            throw new NotImplementedException();
        }

        public void setReturnNewIdWhenInsert(bool value)
        {
            throw new NotImplementedException();
        }

        public bool Update(object entity, string fields = null)
        {
            throw new NotImplementedException();
        }
    }
}