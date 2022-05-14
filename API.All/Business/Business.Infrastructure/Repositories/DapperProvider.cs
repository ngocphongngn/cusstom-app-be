using Business.AppCore.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Business.Infrastructure.BaseRepositories
{
    public abstract class DapperProvider : IDatabaseProvider
    {
        public virtual void CloseConnection(IDbConnection connection)
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        public List<T> ExecuteQueryObject<T>(string storeName, object param = null)
        {
            IDbConnection cnn = null;
            try
            {
                cnn = GetOpenConnection();
                return this.ExecuteQueryObject<T>(cnn, storeName, param);
            }
            finally
            {
                CloseConnection(cnn);
            }
        }
        public List<T> ExecuteQueryObject<T>(IDbConnection cnn, string storeName, object param = null)
        {
            var dynamicParams = BuildParams(storeName, cnn, param);
            var result = cnn.Query<T>(storeName, dynamicParams, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        private DynamicParameters BuildParams(string storeName, IDbConnection cnn, object entity, IDbTransaction transaction = null)
        {
            var dynamicParameters = new DynamicParameters();
            var entityType = entity.GetType();
            var paramArray = DeriveParameters(cnn, storeName, transaction);
            foreach (var p in paramArray)
            {
                if (p != null)
                {
                    var name = p.ParameterName;
                    name = name.Replace(name.Contains("$") ? "@$" : "@", "");
                    object value = null;
                    var pr = entityType.GetProperty(name);
                    if (pr != null)
                    {
                        value = pr.GetValue(entity, null);
                    }
                    dynamicParameters.Add(p.ParameterName, value, this.ToDbType(pr.PropertyType), direction: p.Direction);
                }
            }
            return dynamicParameters;
        }
        protected virtual List<IDataParameter> DeriveParameters(IDbConnection cnn, string storeName, IDbTransaction DeriveParameters = null)
        {
            throw new NotImplementedException();
        }
        public DbType ToDbType(Type type)
        {
            if (type.IsEnum)
            {
                return DbType.Int32;
            }
            DbTypeMapEntry entry = Find(type);
            return entry.DbType;
        }
        private DbTypeMapEntry Find(Type type)
        {
            foreach (var entry in _DbTypeList)
            {
                if (entry.Type == type)
                {
                    return entry;
                }
            }
            throw new ApplicationException("Referenced an unsupported type");
        }

        private static List<DbTypeMapEntry> _DbTypeList = new List<DbTypeMapEntry>()
        {
            new DbTypeMapEntry(typeof(bool), DbType.Boolean, SqlDbType.Bit),
            new DbTypeMapEntry(typeof(bool?), DbType.Boolean, SqlDbType.Bit),
            new DbTypeMapEntry(typeof(byte), DbType.Double, SqlDbType.TinyInt),
            new DbTypeMapEntry(typeof(byte[]), DbType.Binary, SqlDbType.Image),
            new DbTypeMapEntry(typeof(byte[]), DbType.Binary, SqlDbType.Binary),
            new DbTypeMapEntry(typeof(DateTime), DbType.DateTime, SqlDbType.DateTime),
            new DbTypeMapEntry(typeof(DateTime?), DbType.DateTime, SqlDbType.DateTime),
            new DbTypeMapEntry(typeof(Decimal), DbType.Decimal, SqlDbType.Decimal),
            new DbTypeMapEntry(typeof(Decimal), DbType.Decimal, SqlDbType.Decimal),
            new DbTypeMapEntry(typeof(double), DbType.Double, SqlDbType.Float),
            new DbTypeMapEntry(typeof(decimal?), DbType.Decimal, SqlDbType.Decimal),
            new DbTypeMapEntry(typeof(double?), DbType.Double, SqlDbType.Float),
            new DbTypeMapEntry(typeof(Guid), DbType.Guid, SqlDbType.UniqueIdentifier),
            new DbTypeMapEntry(typeof(Guid?), DbType.Guid, SqlDbType.UniqueIdentifier),
            new DbTypeMapEntry(typeof(Int16), DbType.Int16, SqlDbType.SmallInt),
            new DbTypeMapEntry(typeof(Int16), DbType.Int16, SqlDbType.SmallInt),
            new DbTypeMapEntry(typeof(Int32), DbType.Int32, SqlDbType.Int),
            new DbTypeMapEntry(typeof(Int64), DbType.Int64, SqlDbType.BigInt),
            new DbTypeMapEntry(typeof(int?), DbType.Int32, SqlDbType.Int),
            new DbTypeMapEntry(typeof(object), DbType.Object, SqlDbType.Variant),
            new DbTypeMapEntry(typeof(object), DbType.Object, SqlDbType.Variant),
            new DbTypeMapEntry(typeof(string), DbType.String, SqlDbType.VarChar)
        };

        public virtual IDbConnection GetConnection()
        {
            throw new NotImplementedException();
        }

        public string GetConnectionString()
        {
            var cnn = GetConnection();
            return cnn.ConnectionString;
        }

        public IDbConnection GetOpenConnection()
        {
            var cnn = GetConnection();
            cnn.Open();
            return cnn;
        }
    }
}
