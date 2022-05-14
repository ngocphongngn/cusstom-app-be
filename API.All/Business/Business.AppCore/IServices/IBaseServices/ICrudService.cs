using Common.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.AppCore.IServices.IBaseServices
{
    public interface ICrudService<TEntity> : IBaseService
        where TEntity : IRecordState
    {
        Task<TEntity> GetNew(string param);
        Task<List<TEntity>> GetAll();
        Task<TEntity> GetEdit(string id);
    }
}
