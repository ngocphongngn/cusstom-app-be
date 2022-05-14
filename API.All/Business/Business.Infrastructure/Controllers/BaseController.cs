using Common.Model.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Infrastructure.BaseControllers
{
    [Route("[controller]")]
    [ApiController]
    public abstract class BaseControler : ControllerBase
    {
        protected readonly ILogger _log;
        public BaseControler(ILogger log)
        {
            _log = log;
        }

        /// <summary>
        /// Xử lý exception cho các api
        /// </summary>
        protected virtual IActionResult HandleException(Exception ex)
        {
            LogException(ex);
            var msg = "Exception";
#if DEBUG
            msg = ex.ToString();
#endif
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }

        /// <summary>
        /// Xử lý exception cho các api
        /// </summary>
        protected virtual IActionResult HandleException(Exception ex, string message)
        {
            LogException(ex, message);
            var msg = "Exception";
#if DEBUG
            msg = ex.ToString();
#endif
            return StatusCode(StatusCodes.Status500InternalServerError, msg);
        }

        /// <summary>
        /// Xử lý exception cho các api
        /// </summary>
        protected virtual IActionResult HandleException(Exception ex, ServiceResult result)
        {
            LogException(ex);
            result.OnException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        /// <summary>
        /// Cập nhật lỗi NLog
        /// </summary>
        protected void LogException(Exception ex, string message = null)
        {
            _log.LogError(ex, $"{message ?? ex.Message}");
        }

        protected virtual IActionResult ResponseResult(ServiceResult result, int statusCode = 500)
        {
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(statusCode, result);
            }
        }
    }

}

