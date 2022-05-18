using Business.AppCore.IServices.IBaseServices;
using Business.Infrastructure.BaseControllers;
using Common.Model.Base;
using Common.Utility.Services;
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
    }
}
