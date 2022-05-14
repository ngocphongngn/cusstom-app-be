using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Business.Infrastructure.BaseRepositories
{
    public class MySqlProvider : DapperProvider
    {
        private readonly string _connectionString;
        public MySqlProvider(string connectionString)
        {
            _connectionString = connectionString;
        }
        public override IDbConnection GetConnection()
        {
            var cnn = new MySqlConnection(_connectionString);
            return cnn;
        }
    }
}
