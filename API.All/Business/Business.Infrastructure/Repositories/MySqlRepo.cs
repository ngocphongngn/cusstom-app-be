using Business.AppCore.IRepositories;
using Common.Model.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Infrastructure.BaseRepositories
{
    public class MySqlRepo : BaseRepo
    {
        public MySqlRepo(string connectionString) : base(connectionString)
        {
        }
        protected override IDatabaseProvider CreateProvider(string connectionString)
        {
            return new MySqlProvider(connectionString);
        }
    }
}
