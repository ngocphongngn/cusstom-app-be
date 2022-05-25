using Business.AppCore.IServices.IBaseServices;
using Business.Infrastructure.BaseControllers;
using Common.Model.Base;
using Common.Model.Parameter;
using Common.Utility.Services;
using Common.Constant.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Infrastructure.Controllers
{
    //[Authorize]
    public abstract class BaseBusinessController<TEntity, TService> : BaseControler
        where TEntity : IRecordState
        where TService : ICrudService<TEntity>
    {
        protected readonly TService _service;
        protected readonly IParamService _paramService;
        protected readonly static Type EntityType = typeof(TEntity);

        public BaseBusinessController(TService service, ILogger log, IParamService paramService) : base(log)
        {
            _service = service;
            _paramService = paramService;
        }
        /// <summary>
        /// Lấy dữ liệu theo id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEdit(string id)
        {
            try
            {
                var data = await _service.GetEdit(id);
                if (data == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Khong tim thay ban ghi {id}");
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpPost]
        public async virtual Task<IActionResult> Insert(SaveParameter<TEntity> parameter)
        {
            var result = new ServiceResult();
            try
            {
                parameter.Entity.EntityState = Common.Constant.Enums.ModelState.Insert;
                result = await _service.SaveAsync(parameter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpPut]
        public async virtual Task<IActionResult> Update(SaveParameter<TEntity> parameter)
        {
            var result = new ServiceResult();
            try
            {
                parameter.Entity.EntityState = Common.Constant.Enums.ModelState.Update;
                result = await _service.SaveAsync(parameter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpDelete]
        public async virtual Task<IActionResult> Delete(DeleteParameter<List<TEntity>> parameter)
        {
            try
            {
                var result = await _service.DeleteAsync(parameter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

    }
}
