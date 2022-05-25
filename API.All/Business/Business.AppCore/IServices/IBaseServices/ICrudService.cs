using Common.Model.Base;
using Common.Model.Parameter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.AppCore.IServices.IBaseServices
{
    public interface ICrudService<TEntity> : IBaseService
        where TEntity : IRecordState
    {
        /// <summary>
        /// Lấy dữ liệu thêm mới
        /// </summary>
        Task<TEntity> GetNew(string param);
        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        Task<List<TEntity>> GetAll();
        /// <summary>
        /// Lấy theo id
        /// </summary>
        Task<TEntity> GetEdit(string id);
        /// <summary>
        /// Cất dữ liệu thêm/sửa
        /// </summary>
        Task<ServiceResult> SaveAsync(SaveParameter<TEntity> parameter);
        /// <summary>
        /// Thêm/sửa nhiều dữ liệu
        /// </summary>
        Task<ServiceResult> BatchInsert(SaveParameter<List<TEntity>> parameter);
        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        Task<ServiceResult> DeleteAsync(DeleteParameter<TEntity> parameter);
        /// <summary>
        /// Xóa nhiều dữ liệu
        /// </summary>
        Task<ServiceResult> DeleteAsync(DeleteParameter<List<TEntity>> parameter);
    }
}
