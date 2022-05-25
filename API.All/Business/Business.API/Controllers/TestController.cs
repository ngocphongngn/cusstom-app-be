using Business.AppCore.IServices.IBusinessServices;
using Business.Infrastructure.Controllers;
using Common.Model.Business;
using Common.Utility.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.API.Controllers
{
    public class TestController : BaseBusinessController<Test, ITestService>
    {
        public TestController(
            ILogger<TestController> log,
            ITestService service,
            IParamService paramService) : base(service, log, paramService)
        {
        } 
    }
}
