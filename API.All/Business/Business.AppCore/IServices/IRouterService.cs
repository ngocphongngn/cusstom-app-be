using System;
using System.Collections.Generic;
using System.Text;

namespace Business.AppCore.IServices
{
    public interface IRouterService
    {
        string GetEnvName(string version);
    }
}
