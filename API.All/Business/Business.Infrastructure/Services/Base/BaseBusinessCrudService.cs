using Business.AppCore.IServices.IBaseServices;
using Common.Constant.Enums;
using Common.Model.Base;
using Common.Model.Business;
using Common.Model.Config;
using Common.Model.Parameter;
using Common.Utility.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        /// <summary>
        /// Insert nhiều
        /// </summary>
        public virtual async Task<ServiceResult> BatchInsert(SaveParameter<List<TEntity>> parameter)
        {
            IDbConnection cnn = null;
            try
            {
                //open connection
                cnn = _repo.Provider.GetOpenConnection();
                if(parameter.Entity.Count == 1)
                {
                    return await this.SaveAsync(cnn, new SaveParameter<TEntity> {
                        Entity = parameter.Entity.First(),
                        ByPassValidate = parameter.ByPassValidate
                    });
                }
                else
                {
                    var result = new ServiceResult();
                    var errorData = new List<object>();
                    foreach(var item in parameter.Entity)
                    {
                        var itemParam = new SaveParameter<TEntity> {
                            Entity = item,
                            ByPassValidate = parameter.ByPassValidate
                        };
                        var itemResult = await this.SaveAsync(cnn, itemParam);
                        if (!itemResult.Success)
                        {
                            errorData.Add(new {
                                Entity = item,
                                Data = itemResult.Data,
                                Message = itemResult.UserMessage
                            });
                        }
                    }
                    if(errorData.Count > 0)
                    {
                        result.Data = errorData;
                        result.Code = Common.Constant.Enums.ServiceResponseCode.PartInvalidData;
                    }
                    return result;
                }
            }
            finally
            {
                //close connection
                _repo.Provider.CloseConnection(cnn);
            }
        }

        public Task<ServiceResult> DeleteAsync(DeleteParameter<TEntity> parameter)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> DeleteAsync(DeleteParameter<List<TEntity>> parameter)
        {
            throw new NotImplementedException();
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
            var pr = _typeService.GetKeyProperty(type);
            if(pr != null)
            {
                var masterId = JsonConvert.DeserializeObject(id, pr.PropertyType);
                cnn = _repo.Provider.GetOpenConnection();
                var param = new Dictionary<string, object>();
                param.Add(pr.Name, masterId);
                var customSql = await this.GetSourceSql(EntityType);
                if (!string.IsNullOrEmpty(customSql))
                {
                    entity = _repo.Provider.Query<TEntity>(customSql, new Dictionary<string, object> { { "key", masterId } }).FirstOrDefault();
                }
                else
                {
                    entity = _repo.GetById<TEntity>(cnn, masterId);
                }
            }
            return entity;
        }
        protected virtual async Task<string> GetSourceSql(Type type)
        {
            //to do get file sql
            return null;
        }
        public Task<TEntity> GetNew(string param)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<ServiceResult> SaveAsync(SaveParameter<TEntity> parameter)
        {
            var result = new ServiceResult();
            IDbConnection cnn = null;
            try
            {
                cnn = _repo.Provider.GetOpenConnection();
                result = await this.SaveAsync(cnn, parameter);
            }
            finally
            {
                _repo.Provider.CloseConnection(cnn);
            }
            return result;
        }
        public virtual async Task<ServiceResult> SaveAsync(IDbConnection cnn, SaveParameter<TEntity> parameter)
        {
            var result = new ServiceResult();
            try
            {
                //to do process before save
                //await this.BeforeSave(cnn, parameter);
                //to do validate
                //var validateResult = await this.ValidateSave(cnn, parameter);
                result = await this.SaveData(cnn, parameter);
                if(result.Code == Common.Constant.Enums.ServiceResponseCode.Success)
                {
                    //to do process after save
                    //wait this.AfterSave(cnn, parameter, result);
                }
                else
                {
                    result.OnError(result.Code, result.SubCode, result.Data, null, result.SystemMessage);
                }
            }
            catch(Exception ex)
            {
                result.OnException(ex);
            }
            return result;
        }
        protected virtual async Task<ServiceResult> SaveData(IDbConnection cnn, SaveParameter<TEntity> parameter)
        {
            IDbTransaction transaction = null;
            ServiceResult result = new ServiceResult();
            try
            {
                using(transaction = cnn.BeginTransaction())
                {
                    //nếu có lỗi thì dừng lại
                    if(result.Code != Common.Constant.Enums.ServiceResponseCode.Success)
                    {
                        return result;
                    }
                    await this.UpdateDataItem(transaction, parameter.Entity);
                    if(result.Code == Common.Constant.Enums.ServiceResponseCode.Success)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        this.RollBackTransaction(transaction);
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                this.RollBackTransaction(transaction);
                throw;
            }
            return result;
        }
        protected async virtual Task UpdateDataItem(IDbTransaction transaction, object data)
        {
            ModelState state = ModelState.None;
            if(data is IRecordState)
            {
                state = (data as IRecordState).EntityState;
            }
            switch (state)
            {
                case ModelState.Insert:
                    _repo.Insert(transaction, data);
                    break;
                case ModelState.Update:
                    _repo.Insert(transaction, data);
                    break;
                case ModelState.Delete:
                    _repo.Insert(transaction, data);
                    break;
            }
        }
        /// <summary>
        /// Rollback lại transaction
        /// </summary>
        protected void RollBackTransaction(IDbTransaction transaction)
        {
            try
            {
                if(transaction != null)
                {
                    transaction.Rollback();
                }
            }
            catch(Exception)
            {

            }
        }
    }
}
