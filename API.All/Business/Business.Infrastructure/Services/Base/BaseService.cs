using Business.AppCore.IRepositories;
using Business.AppCore.IServices.IBaseServices;
using Business.Infrastructure.BaseRepositories;
using Common.Model.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Infrastructure.Services.Base
{
    public class BaseService : IBaseService
    {
        private IBaseRepo __repo;
        public IBaseRepo _repo
        {
            get
            {
                if(__repo == null)
                {
                    __repo = this.CreateRepo();
                }
                return __repo;
            }
        }
        protected readonly CenterConfig _config;
        public BaseService(CenterConfig config)
        {
            _config = config;
        }
        public virtual IBaseRepo CreateRepo()
        {
            return new MySqlRepo(this.GetConnectionString());
        }
        public object Insert(object entity)
        {
            return __repo.Insert(entity);
        }
        public bool Update(object entity, string fields = null)
        {
            return __repo.Update(entity, fields);
        }
        public List<T> Get<T>(string field, object value, string op = "=")
        {
            return _repo.Get<T>(field, value, op);
        }
        public virtual string GetConnectionString()
        {
            throw new NotImplementedException();
        }
        public void Delete(object entity)
        {
            _repo.Delete(entity);
        }

    }
}
