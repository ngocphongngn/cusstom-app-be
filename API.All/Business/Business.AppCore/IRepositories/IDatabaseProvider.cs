using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Business.AppCore.IRepositories
{
    public interface IDatabaseProvider
    {
        string GetConnectionString();
        IDbConnection GetConnection();
        IDbConnection GetOpenConnection();
        void CloseConnection(IDbConnection connection);
        List<T> ExecuteQueryObject<T>(string storeName, object param = null);
    }
}
