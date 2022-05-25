using Business.AppCore.IRepositories;
using Common.Utility.Utils;
using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Business.Infrastructure.BaseRepositories
{
    public abstract class DapperProvider : IDatabaseProvider
    {
        /// <summary>
        /// Các từ check sql injection
        /// </summary>
        private static string[] _InjectionWords = new string[] {
            "--",
            "#",
            "\\/*",
            "*\\/",
            "grant ",
            "drop ",
            "truncate ",
            "sleep( ",
            "exec ",
            "execute ",
            "prepare ",
            "information_schema",
            "delay ",
        };
        /// <summary>
        /// Các từ khóa bỏ qua khi kiểm tra
        /// </summary>
        private static string[] _InjectionIgnoreWords = new string[] {
            "''",
            "'MORE_DETAILS'",
            "'$'", "\"$\"",
            "'%'", "\"%\"",
            "','", "\",\"",
            "'\", \"'", "'\", \"'",
            "'\\[\"'",
            "'\"\\]'",
            "\"$[*]\"", @"\'$[*]\'",
            @"\'\$\.[a-zA-Z0-9]+\'", "\"\\$\\.[a-zA-Z0-9]+\"",
            @"'null'", "\"null\"",
            "\"ONLY_FULL_GROUP_BY,NO_UNSIGNED_SUBTRACTION,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION\"",
            "'Other'",
            "'#'",
            "Drop TEMPORARY table"
        };
        private static string _InjecttionIgnoreWordsReplace = " ";

        /// <summary>
        /// Các từ khóa bị remove khỏi sql
        /// </summary>
        private static Dictionary<string, string> _InjectionRemoveWords = new Dictionary<string, string>
        {
            { "\\/\\*(.*)\\*\\/", ""}
        };

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

        public int ExecuteNonQueryText(string commandText, Dictionary<string, object> param, int? timeout = null)
        {
            IDbConnection cnn = null;
            try
            {
                cnn = GetOpenConnection();
                return this.ExecuteNonQueryText(cnn, commandText, param, timeout: timeout);
            }
            finally
            {
                CloseConnection(cnn);
            }
        }

        public int ExecuteNonQueryText(IDbConnection cnn, string commandText, Dictionary<string, object> param, int? timeout = null)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var dynamicParams = new DynamicParameters();
            if(param != null)
            {
                foreach(var item in param)
                {
                    dynamicParams.Add(item.Key, value: item.Value);
                }
            }
            var result = cnn.Execute(sql, dynamicParams, commandType: CommandType.Text, commandTimeout: timeout);
            return result;
        }

        public int ExecuteNonQueryText(IDbTransaction transaction, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var dynamicParams = new DynamicParameters();
            if (param != null)
            {
                foreach (var item in param)
                {
                    dynamicParams.Add(item.Key, value: item.Value);
                }
            }
            var result = transaction.Connection.Execute(sql, dynamicParams, commandType: CommandType.Text, transaction: transaction);
            return result;
        }
        public int ExecuteNonQueryText(string commandText, object param)
        {
            IDbConnection cnn = null;
            try
            {
                cnn = GetOpenConnection();
                return this.ExecuteNonQueryText(cnn, commandText, param);
            }
            finally
            {
                CloseConnection(cnn);
            }
        }
        public int ExecuteNonQueryText(IDbConnection cnn, string commandText, object param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = cnn.Execute(sql, param, commandType: CommandType.Text);
            return result;
        }
        public int ExecuteNonQueryText(IDbTransaction transaction, string commandText, object param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = transaction.Connection.Execute(sql, param, commandType: CommandType.Text, transaction:transaction);
            return result;
        }
        public object ExecuteScalarText(string commandText, Dictionary<string, object> param)
        {
            IDbConnection cnn = null;
            try
            {
                cnn = GetOpenConnection();
                return this.ExecuteScalarText(cnn, commandText, param);
            }
            finally
            {
                CloseConnection(cnn);
            }
        }

        public object ExecuteScalarText(IDbConnection cnn, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var dynamicParams = new DynamicParameters();
            if (param != null)
            {
                foreach (var item in param)
                {
                    dynamicParams.Add(item.Key, value: item.Value);
                }
            }
            var result = cnn.ExecuteScalar(sql, dynamicParams, commandType: CommandType.Text);
            return result;
        }

        public object ExecuteScalarText(IDbTransaction transaction, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var dynamicParams = new DynamicParameters();
            if (param != null)
            {
                foreach (var item in param)
                {
                    dynamicParams.Add(item.Key, value: item.Value);
                }
            }
            var result = transaction.Connection.ExecuteScalar(sql, dynamicParams, commandType: CommandType.Text, transaction: transaction);
            return result;
        }
        public object ExecuteScalarText(string commandText, object param)
        {
            IDbConnection cnn = null;
            try
            {
                cnn = GetOpenConnection();
                return this.ExecuteScalarText(cnn, commandText, param);
            }
            finally
            {
                CloseConnection(cnn);
            }
        }
        public object ExecuteScalarText(IDbConnection cnn, string commandText, object param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = cnn.ExecuteScalar(sql, param, commandType: CommandType.Text);
            return result;
        }
        public object ExecuteScalarText(IDbTransaction transaction, string commandText, object param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var result = transaction.Connection.ExecuteScalar(sql, param, commandType: CommandType.Text, transaction: transaction);
            return result;
        }
        public List<T> Query<T>(string commandText, Dictionary<string, object> param)
        {
            IDbConnection cnn = null;
            try
            {
                cnn = GetOpenConnection();
                return this.Query<T>(cnn, commandText, param);
            }
            finally
            {
                CloseConnection(cnn);
            }
        }
        public List<T> Query<T>(IDbConnection cnn, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var dynamicParams = new DynamicParameters();
            if(param != null)
            {
                foreach(var item in param)
                {
                    dynamicParams.Add(item.Key, value: item.Value);
                }
            }
            var result = cnn.Query<T>(sql, dynamicParams, commandType: CommandType.Text);
            return result.AsList();
        }
        public List<T> Query<T>(IDbTransaction transaction, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var dynamicParams = new DynamicParameters();
            if (param != null)
            {
                foreach (var item in param)
                {
                    dynamicParams.Add(item.Key, value: item.Value);
                }
            }
            var result = transaction.Connection.Query<T>(sql, dynamicParams, commandType: CommandType.Text, transaction: transaction);
            return result.AsList();
        }
        public IList Query(Type type, string commandText, Dictionary<string, object> param)
        {
            IDbConnection cnn = null;
            try
            {
                cnn = GetOpenConnection();
                return this.Query(cnn, type, commandText, param);
            }
            finally
            {
                CloseConnection(cnn);
            }
        }
        public IList Query(IDbConnection cnn, Type type, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var dynamicParams = new DynamicParameters();
            if (param != null)
            {
                foreach (var item in param)
                {
                    dynamicParams.Add(item.Key, value: item.Value);
                }
            }
            var data = cnn.Query(type, sql, dynamicParams, commandType: CommandType.Text) as IList;
            var result = TypeUtil.CreateList(type);
            foreach(var item in data)
            {
                result.Add(item);
            }
            return result;
        }
        public IList Query(IDbTransaction transaction, Type type, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var dynamicParams = new DynamicParameters();
            if (param != null)
            {
                foreach (var item in param)
                {
                    dynamicParams.Add(item.Key, value: item.Value);
                }
            }
            var data = transaction.Connection.Query(type, sql, dynamicParams, commandType: CommandType.Text, transaction: transaction) as IList;
            var result = TypeUtil.CreateList(type);
            foreach (var item in data)
            {
                result.Add(item);
            }
            return result;
        }
        public IEnumerable<dynamic> Query(string commandText, Dictionary<string, object> param)
        {
            IDbConnection cnn = null;
            try
            {
                cnn = GetOpenConnection();
                return this.Query(cnn, commandText, param);
            }
            finally
            {
                CloseConnection(cnn);
            }
        }

        public IEnumerable<dynamic> Query(IDbConnection cnn, string commandText, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var dynamicParams = new DynamicParameters();
            if (param != null)
            {
                foreach (var item in param)
                {
                    dynamicParams.Add(item.Key, value: item.Value);
                }
            }
            var data = cnn.Query(sql, dynamicParams, commandType: CommandType.Text);
            return data;
        }

        public List<T> QueryWithCommandType<T>(IDbConnection cnn, string commandText, CommandType pCommandType, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var dynamicParams = new DynamicParameters();
            if (param != null)
            {
                foreach (var item in param)
                {
                    dynamicParams.Add(item.Key, value: item.Value);
                }
            }
            var data = cnn.Query<T>(sql, dynamicParams, commandType: pCommandType);
            return data.ToList();
        }

        public List<T> QueryWithCommandType<T>(IDbTransaction transaction, string commandText, CommandType pCommandType, Dictionary<string, object> param)
        {
            var sql = this.ProcessSqlBeforExecute(commandText);
            var dynamicParams = new DynamicParameters();
            if (param != null)
            {
                foreach (var item in param)
                {
                    dynamicParams.Add(item.Key, value: item.Value);
                }
            }
            var data = transaction.Connection.Query<T>(sql, dynamicParams, commandType: pCommandType);
            return data.ToList();
        }
        /// <summary>
        /// Xử lý câu lệnh trc khi exe
        /// </summary>
        public string ProcessSqlBeforExecute(string sql)
        {
            var result = sql;
            if(_InjectionRemoveWords != null)
            {
                foreach(var item in _InjectionRemoveWords)
                {
                    result = Regex.Replace(result, item.Key, item.Value);
                }
            }
            this.ValidateSqlInjection(result);
            return result;
        }
        /// <summary>
        /// Check sql injection
        /// </summary>
        private void ValidateSqlInjection(string sql)
        {
            var checkSql = sql;
            if(_InjectionIgnoreWords != null)
            {
                foreach(var item in _InjectionIgnoreWords)
                {
                    checkSql = Regex.Replace(checkSql, item, _InjecttionIgnoreWordsReplace, RegexOptions.IgnoreCase);
                }
            }
            foreach(var item in _InjectionWords)
            {
                if(checkSql.IndexOf(item, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    throw new Exception($"Query invalid {item}: {checkSql}");
                }
            }
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
        public virtual void CloseConnection(IDbConnection connection)
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

    }
}
