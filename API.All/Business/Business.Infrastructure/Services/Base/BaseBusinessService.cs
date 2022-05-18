using Common.Model.Config;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Business.Infrastructure.Services.Base
{
    public class BaseBusinessService : BaseService
    {
        protected readonly static ConcurrentDictionary<int, string> _connections = new ConcurrentDictionary<int, string>();
        public BaseBusinessService(CenterConfig config) : base(config)
        {
        }
        public override string GetConnectionString()
        {
            return _config.Connections.App;
        }
    }
}
