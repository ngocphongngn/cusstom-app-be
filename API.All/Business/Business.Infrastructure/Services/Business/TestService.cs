using Business.AppCore.IServices.IBusinessServices;
using Business.Infrastructure.Services.Base;
using Common.Model.Business;
using Common.Model.Config;
using Common.Utility.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Infrastructure.Services.Business
{
    public class TestService : BaseBusinessCrudService<Test>, ITestService
    {
        public TestService(CenterConfig config, ITypeService typeService) : base(config, typeService)
        {

        }
    }
}
