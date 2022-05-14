using Business.AppCore.IServices.IBaseServices;
using Common.Model.Base;
using Common.Model.Config;
using Common.Utility.Services;
using System;
using System.Collections.Generic;
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

        public Task<TEntity> GetEdit(string id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetNew(string param)
        {
            throw new NotImplementedException();
        }
    }
}
