using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Business.Infrastructure.BaseRepositories
{
    internal struct DbTypeMapEntry
    {
        public Type Type;
        public DbType DbType;
        public SqlDbType SqlDbType;
        public DbTypeMapEntry(Type type, DbType dbType, SqlDbType sqlDbType)
        {
            this.Type = type;
            this.DbType = dbType;
            this.SqlDbType = sqlDbType;
        }
    }
}
