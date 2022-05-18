using Business.AppCore.IRepositories;
using Common.Model.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Business.Infrastructure.BaseRepositories
{
    public class BaseRepo : IBaseRepo
    {
        protected readonly IDatabaseProvider _dbProvider;
        /// <summary>
        /// Trả về giá trị khóa chính tự tăng khi insert
        /// </summary>
        private bool _returnNewIdWhenInsert = true;
        public IDatabaseProvider Provider => _dbProvider;
        public BaseRepo(string connectionString)
        {
            this._dbProvider = this.CreateProvider(connectionString);
        }

        protected virtual IDatabaseProvider CreateProvider(string connectionString)
        {
            throw new NotImplementedException();
        }

        public string buildQueryById<T>()
        {
            throw new NotImplementedException();
        }

        public bool Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(IDbConnection cnn, object entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(IDbTransaction transaction, object entity)
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
            var query = $"Select * from {typeof(T).Name}";
            Dictionary<string, object> param = null;
            //var result = _dbProvider.Query<T>(query, param);
            var result = new List<T>();
            return result;
        }

        public List<T> Get<T>(IDbConnection cnn)
        {
            var query = $"Select * from {typeof(T).Name}";
            Dictionary<string, object> param = null;
            //var result = _dbProvider.Query<T>(cnn, query, param);
            var result = new List<T>();
            return result;
        }

        public T GetById<T>(object id)
        {
            throw new NotImplementedException();
        }

        public T GetById<T>(IDbConnection cnn, object id)
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

        public void Submit(List<ModelData> data)
        {
            throw new NotImplementedException();
        }

        public void Submit(IDbConnection cnn, List<ModelData> data)
        {
            throw new NotImplementedException();
        }

        public void Submit(IDbTransaction transaction, List<ModelData> data)
        {
            throw new NotImplementedException();
        }

        public bool Update(object entity, string fields = null)
        {
            throw new NotImplementedException();
        }

        public bool Update(IDbConnection cnn, object entity, string fields = null)
        {
            throw new NotImplementedException();
        }

        public bool Update(IDbTransaction transaction, object entity, string fields = null)
        {
            throw new NotImplementedException();
        }
    }
}