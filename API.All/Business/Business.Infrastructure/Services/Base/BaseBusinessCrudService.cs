using Business.AppCore.IServices.IBaseServices;
using Common.Model.Base;
using Common.Model.Business;
using Common.Model.Config;
using Common.Utility.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Business.Infrastructure.Services.Base
{
    public class BaseBusinessCrudService<TEntity> : BaseBusinessService, ICrudService<TEntity> where TEntity : IRecordState
    {
        public static readonly Type EntityType = typeof(TEntity);
        protected readonly ITypeService _typeService;
        public BaseBusinessCrudService(
            CenterConfig config,
            ITypeService typeService) : base(config)
        {
            _typeService = typeService;
        }

        public Task<List<TEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> GetEdit(string id)
        {
            IDbConnection cnn = null;
            try{
                //open connection
                cnn = _repo.Provider.GetOpenConnection();
                return await this.GetEdit(cnn, id);
            }
            finally
            {
                //close connection
                _repo.Provider.CloseConnection(cnn);
            }
        }
        public virtual async Task<TEntity> GetEdit(IDbConnection cnn, string id)
        {
            TEntity entity = default(TEntity);
            var type = typeof(TEntity);
            //var pr = _typeService.GetKeyPropertyType();
            cnn = _repo.Provider.GetOpenConnection();
            return entity;

        }

        public Task<TEntity> GetNew(string param)
        {
            throw new NotImplementedException();
        }
    }
}
