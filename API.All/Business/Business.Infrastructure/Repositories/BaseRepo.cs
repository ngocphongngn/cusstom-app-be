using Business.AppCore.IRepositories;
using Common.Model.Attributes;
using Common.Model.Base;
using Common.Model.Other;
using Common.Utility.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Common.Constant.Enums;

namespace Business.Infrastructure.BaseRepositories
{
    public class BaseRepo : IBaseRepo
    {
        protected readonly IDatabaseProvider _dbProvider;
        /// <summary>
        /// Trả về giá trị khóa chính tự tăng khi insert
        /// </summary>
        private bool _returnNewIdWhenInsert = true;
        public IDatabaseProvider Provider => _dbProvider;

        //contructor
        public BaseRepo(string connectionString)
        {
            this._dbProvider = this.CreateProvider(connectionString);
        }
        /// <summary>
        /// Khởi tạo provider thao tác với db
        /// </summary>
        protected virtual IDatabaseProvider CreateProvider(string connectionString)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Tùy biến query get dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual string buildQueryById<T>()
        {
            var type = typeof(T);
            var prKey = TypeUtil.GetKeyProperty(typeof(T));
            return $"Select * from {type.Name} where {prKey.Name} = @key";
        }

        public bool Delete(object entity)
        {
            var query = this.GetDeleteQuery(entity);
            var res = _dbProvider.ExecuteNonQueryText(query, entity);
            return res > 0;
        }

        public bool Delete(IDbConnection cnn, object entity)
        {
            var query = this.GetDeleteQuery(entity);
            var res = _dbProvider.ExecuteNonQueryText(cnn, query, entity);
            return res > 0;
        }

        public bool Delete(IDbTransaction transaction, object entity)
        {
            var query = this.GetDeleteQuery(entity);
            var res = _dbProvider.ExecuteNonQueryText(transaction, query, entity);
            return res > 0;
        }
        private static readonly Dictionary<Type, string> _deleteCommands = new Dictionary<Type, string>();
        /// <summary>
        /// Gen sql xóa dữ liệu
        /// </summary>
        private string GetDeleteQuery(object entity)
        {
            var type = entity.GetType();
            if (!_deleteCommands.ContainsKey(type))
            {
                var key = TypeUtil.GetKeyProperty(type);
                _deleteCommands[type] = $"Delete from {type.Name} where {key.Name} = @{key.Name};";
            }
            return _deleteCommands[type];
        }
        public List<T> Get<T>(string field, object value, string op = "=")
        {
            var safeOp = this.SafeOperator(op);
            var param = new Dictionary<string, object>();
            var sql = this.BuildSelectByFieldQuery<T>(param, field, value, op = safeOp);
            var result = _dbProvider.Query<T>(sql, param);
            return result;
        }

        public List<T> Get<T>(IDbConnection cnn, string field, object value, string op = "=", string columns = "*")
        {
            var safeOp = this.SafeOperator(op);
            var param = new Dictionary<string, object>();
            var sql = this.BuildSelectByFieldQuery<T>(param, field, value, op = safeOp);
            var result = _dbProvider.Query<T>(cnn, sql, param);
            return result;
        }

        public List<T> Get<T>(IDbTransaction transaction, string field, object value, string op = "=")
        {
            var safeOp = this.SafeOperator(op);
            var param = new Dictionary<string, object>();
            var sql = this.BuildSelectByFieldQuery<T>(param, field, value, op = safeOp);
            var result = _dbProvider.Query<T>(transaction, sql, param);
            return result;
        }

        public List<T> Get<T>()
        {
            var query = $"Select * from {typeof(T).Name}";
            Dictionary<string, object> param = null;
            var result = _dbProvider.Query<T>(query, param);
            return result;
        }
        public T GetById<T>(object id)
        {
            var sql = this.buildQueryById<T>();
            var result = this._dbProvider.Query<T>(sql, new Dictionary<string, object> { { "key", id } });
            return result.FirstOrDefault();
        }
        public List<T> Get<T>(IDbConnection cnn)
        {
            var query = $"Select * from {typeof(T).Name}";
            Dictionary<string, object> param = null;
            var result = _dbProvider.Query<T>(cnn, query, param);
            return result;
        }

        
        public T GetById<T>(IDbConnection cnn, object id)
        {
            var sql = this.buildQueryById<T>();
            var result = this._dbProvider.Query<T>(cnn, sql, new Dictionary<string, object> { { "key", id } });
            return result.FirstOrDefault();
        }
        private string BuildSelectByFieldQuery<T>(Dictionary<string, object> param, string field, object value, string op = "=", string columns = "*")
        {
            var safeOp = this.SafeOperator(op);
            var sb = new StringBuilder($"Select {columns} from {typeof(T).Name} where {field} {safeOp} ");
            if(safeOp == "in" || safeOp == "not in")
            {
                IList vlue = (IList)value;
                sb.Append("(");
                for(var i = 0; i < vlue.Count; i++)
                {
                    if(i > 0)
                    {
                        sb.Append(",");
                    }
                    var p = $"p{i}";
                    sb.Append($"@{p}");
                    param[p] = vlue[i];
                }
                sb.Append(")");
            }
            else
            {
                sb.Append("@value");
                param["value"] = value;
            }
            return sb.ToString();
        }
        /// <summary>
        /// Check toán tử
        /// </summary>
        private string SafeOperator(string op)
        {
            if (op.Contains("'") || op.Contains(";"))
            {
                throw new NotImplementedException($"Không hỗ trợ toán tử {op}");
            }
            return op;
        }
        

        public object Insert(object entity)
        {
            var query = this.GetInsertQuery(entity);
            var res = _dbProvider.ExecuteScalarText(query, entity);
            if (_returnNewIdWhenInsert)
            {
                this.updateEntityKey(entity, res);
            }
            return res;
        }

        public object Insert(IDbConnection cnn, object entity)
        {
            var query = this.GetInsertQuery(entity);
            var res = _dbProvider.ExecuteScalarText(cnn, query, entity);
            if (_returnNewIdWhenInsert)
            {
                this.updateEntityKey(entity, res);
            }
            return res;
        }

        public object Insert(IDbTransaction transaction, object entity)
        {
            var query = this.GetInsertQuery(entity);
            var res = _dbProvider.ExecuteScalarText(transaction, query, entity);
            if (_returnNewIdWhenInsert)
            {
                this.updateEntityKey(entity, res);
            }
            return res;
        }
        private static readonly Dictionary<Type, string> _insertCommands = new Dictionary<Type, string>();
        /// <summary>
        /// Gen sql insert db
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected string GetInsertQuery(object entity)
        {
            var type = entity.GetType();
            if (!_insertCommands.ContainsKey(type))
            {
                var fields = TypeUtil.GetTableProperty(type).Select(n => n.Name);
                var tableName = type.Name;
                try
                {
                    var tableAttribute = type.GetCustomAttribute<TableAttribute>();
                    if(tableAttribute != null && !string.IsNullOrWhiteSpace(tableAttribute.Table))
                    {
                        tableName = tableAttribute.Table;
                    }
                }
                catch (Exception)
                {
                    //to do exxception
                }
                var query = $"Insert into {tableName} (`{string.Join("`,`", fields)}`) values(@{string.Join(",@", fields)});";
                if (_returnNewIdWhenInsert)
                {
                    query += "select last_insert_id();";
                }
                _insertCommands[type] = query;
            }
            return _insertCommands[type];
        }

        /// <summary>
        /// Cập nhật khóa chính cho entity sau khi insert
        /// </summary>
        protected void updateEntityKey(object entity, object excuteResult)
        {
            if(excuteResult != null)
            {
                //cập nhật pk tự tăng
                var pkId = TypeUtil.GetKeyProperty(entity.GetType());
                if(pkId != null)
                {
                    if(pkId.PropertyType == typeof(Int32))
                    {
                        pkId.SetValue(entity, Convert.ToInt32(excuteResult));
                    }
                    else if (pkId.PropertyType == typeof(Int64))
                    {
                        pkId.SetValue(entity, Convert.ToInt64(excuteResult));
                    }
                }
            }
        }
        public void setReturnNewIdWhenInsert(bool value)
        {
            _returnNewIdWhenInsert = value;
        }

        public void Submit(List<SubmitModel> data)
        {
            var querys = this.GetSubmitQuery(data);
            foreach(var  query in querys)
            {
                _dbProvider.ExecuteNonQueryText(query.Query, query.Param);
            }
        }

        public void Submit(IDbConnection cnn, List<SubmitModel> data)
        {
            var querys = this.GetSubmitQuery(data);
            foreach (var query in querys)
            {
                _dbProvider.ExecuteNonQueryText(cnn, query.Query, query.Param);
            }
        }

        public void Submit(IDbTransaction transaction, List<SubmitModel> data)
        {
            var querys = this.GetSubmitQuery(data);
            foreach (var query in querys)
            {
                _dbProvider.ExecuteNonQueryText(transaction, query.Query, query.Param);
            }
        }
        /// <summary>
        /// Build query submit vao` db
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<SqlQuery> GetSubmitQuery(List<SubmitModel> data)
        {
            List<SqlQuery> result = new List<SqlQuery>(), temp;
            foreach(var item in data)
            {
                var sb = new StringBuilder();
                switch (item.State)
                {
                    case ModelState.Insert:
                        temp = this.GetSubmitQueryInsert(item);
                        result.AddRange(temp);
                        break;
                    case ModelState.Update:
                        temp = this.GetSubmitQueryUpdate(item);
                        result.AddRange(temp);
                        break;
                    case ModelState.Delete:
                        temp = this.GetSubmitQueryDelete(item);
                        result.AddRange(temp);
                        break;
                }
            }
            return result;
        }
        /// <summary>
        /// Build sql submit to db (insert)
        /// </summary>
        private List<SqlQuery> GetSubmitQueryInsert(SubmitModel item)
        {
            var result = new SqlQuery() {
                Param = new Dictionary<string, object>()
            };
            var sb = new StringBuilder();
            
            var same = true; //các dòng y hệt thì insert multi row
            string[] fields = null;
            if(item.Datas.Count > 1)
            {
                string lastKey = null;
                foreach(var row in item.Datas)
                {
                    var temp = string.Join(",", row.Keys.OrderBy(n => n));
                    if(lastKey == null)
                    {
                        lastKey = temp;
                        fields = row.Keys.ToArray();
                    }
                    else if(temp != lastKey)
                    {
                        same = false;
                        break;
                    }
                }
            }

            var valueMap = new Dictionary<object, string>();
            int paramCount = 0;
            string paramField;
            if (same)
            {
                //insert multi
                sb.Append($"Insert into {item.TableName} ({string.Join(",", fields)}) values");
                for(var i = 0; i < item.Datas.Count; i++)
                {
                    var dataItem = item.Datas[i];
                    if(i > 0)
                    {
                        sb.AppendLine(",(");
                    }
                    else
                    {
                        sb.Append("(");
                    }
                    for (var j = 0; j < fields.Length; j++)
                    {
                        if (j > 0)
                        {
                            sb.Append(",");
                        }

                        var field = fields[j];
                        var value = dataItem[field];
                        if (value == null)
                        {
                            paramField = $"pnu";
                            result.Param[paramField] = value;
                        }else if (valueMap.ContainsKey(value))
                        {
                            paramField = valueMap[value];
                        }
                        else
                        {
                            paramField = $"p{paramCount++}";
                            valueMap[value] = paramField;
                            result.Param[paramField] = value;
                        }
                        sb.Append($"@{paramField}");
                        result.Param[paramField] = value;
                    }
                    sb.AppendLine(")");
                }
                sb.Append(";");
            }
            else
            {
                //insert 1 row
                for(var i = 0; i< item.Datas.Count; i++)
                {
                    var dataItem = item.Datas[i];
                    fields = dataItem.Keys.ToArray();
                    sb.Append($"Insert into {item.TableName} ({string.Join(",", fields)}) values (");
                    for(var j = 0; j < fields.Length; j++)
                    {
                        if (j > 0)
                        {
                            sb.Append(",");
                        }
                        var field = fields[j];
                        var value = dataItem[field];
                        if(value == null)
                        {
                            paramField = $"pnu";
                            result.Param[paramField] = value;
                        }
                        else if(valueMap.ContainsKey(value))
                        {
                            paramField = valueMap[value];
                        }
                        else
                        {
                            paramField = $"p{paramCount++}";
                            valueMap[value] = paramField;
                            result.Param[paramField] = value;
                        }
                        sb.Append($"@{paramField}");
                        result.Param[paramField] = value;
                    }
                    sb.AppendLine(");");
                }
            }
            result.Query = sb.ToString();
            return new List<SqlQuery> { result };

        }
        /// <summary>
        /// Build sql submit to db (update)
        /// </summary>
        private List<SqlQuery> GetSubmitQueryUpdate(SubmitModel item)
        {
            var result = new SqlQuery() {
                Param = new Dictionary<string, object>()
            };
            var sb = new StringBuilder();
            var valueMap = new Dictionary<object, string>();
            int pCount = 0;
            string paramField;

            for (var i = 0; i < item.Datas.Count; i++)
            {
                var data = item.Datas[i];
                int f = 0;
                var sbKeys = new StringBuilder();
                var sbTemp = new StringBuilder();
                foreach (var field in data.Keys)
                {
                    var value = data[field];
                    if (value == null)
                    {
                        paramField = $"pnu";
                        result.Param[paramField] = value;
                    }
                    else if (valueMap.ContainsKey(value))
                    {
                        paramField = valueMap[value];
                    }
                    else
                    {
                        paramField = $"p{pCount++}";
                        valueMap[value] = paramField;
                        result.Param[paramField] = value;
                    }
                    if (!item.KeyFields.Contains(field))
                    {
                        if (f > 0)
                        {
                            sbTemp.Append(",");
                        }
                        f++;
                        if (field.Contains("="))
                        {
                            if (field.Contains("#ReplaceValue#"))
                            {
                                sbTemp.Append(field.Replace("#ReplaceValue#", $"@{paramField}"));
                            }
                            else
                            {
                                sbTemp.Append($"{field}@{paramField}");
                            }
                        }
                        else
                        {
                            sbTemp.Append($"{field}@{paramField}");
                        }
                    }
                    else
                    {
                        if (sbKeys.Length > 0)
                        {
                            sbKeys.Append($" and {field}=@{paramField}");
                        }
                        else
                        {
                            sbKeys.Append($" {field}=@{paramField}");
                        }
                    }
                }
                //có key ms update
                if (sbKeys.Length > 0)
                {
                    sb.Append($"update {item.TableName} set {sbTemp.ToString()} where {sbKeys.ToString()};");
                }
            }
            result.Query = sb.ToString();
            return new List<SqlQuery> { result };
        }
        /// <summary>
        /// Build sql submit to db (delete)
        /// </summary>
        private List<SqlQuery> GetSubmitQueryDelete(SubmitModel item)
        {
            var result = new SqlQuery() {
                Param = new Dictionary<string, object>()
            };
            var sb = new StringBuilder();
            var valueMap = new Dictionary<object, string>();
            int pCount = 0;
            string paramField;

            for (var i = 0; i < item.Datas.Count; i++)
            {
                var data = item.Datas[i];
                var sbKeys = new StringBuilder();
                foreach (var field in item.KeyFields)
                {
                    //nếu thiếu thông tin khóa thì k xử lý
                    if (!data.ContainsKey(field))
                    {
                        sbKeys.Clear();
                        break;
                    }

                    var value = data[field];

                    if (valueMap.ContainsKey(value))
                    {
                        paramField = valueMap[value];
                    }
                    else
                    {
                        paramField = $"p{pCount++}";
                        valueMap[value] = paramField;
                        result.Param[paramField] = value;
                    }
                    if (sbKeys.Length > 0)
                    {
                        sbKeys.Append($" and {field}=@{paramField}");
                    }
                    else
                    {
                        sbKeys.Append($" {field}=@{paramField}");
                    }
                }
                //có key ms delete
                if (sbKeys.Length > 0)
                {
                    sb.Append($"Delete from {item.TableName} where {sbKeys.ToString()};");
                }
                sb.AppendLine(";");
            }
            result.Query = sb.ToString();
            return new List<SqlQuery> { result };
        }

        public bool Update(object entity, string fields = null)
        {
            var query = this.GetUpdateQuery(entity, fields);
            var res = _dbProvider.ExecuteNonQueryText(query, entity);
            return res > 0;
        }

        public bool Update(IDbConnection cnn, object entity, string fields = null)
        {
            var query = this.GetUpdateQuery(entity, fields);
            var res = _dbProvider.ExecuteNonQueryText(cnn, query, entity);
            return res > 0;
        }

        public bool Update(IDbTransaction transaction, object entity, string fields = null)
        {
            var query = this.GetUpdateQuery(entity, fields);
            var res = _dbProvider.ExecuteNonQueryText(transaction, query, entity);
            return res > 0;
        }
        private static readonly Dictionary<Type, Dictionary<string, string>> _updateCommands = new Dictionary<Type, Dictionary<string, string>>();
        /// <summary>
        /// Gen sql update db
        /// </summary>
        private string GetUpdateQuery(object entity, string fields = null)
        {
            if(fields == null)
            {
                fields = string.Empty;
            }
            var type = entity.GetType();
            if (!_updateCommands.ContainsKey(type))
            {
                _updateCommands[type] = new Dictionary<string, string>();
            }
            if (!_updateCommands[type].ContainsKey(fields))
            {
                var prFields = TypeUtil.GetTableProperty(type);
                var key = TypeUtil.GetKeyProperty(type);
                List<PropertyInfo> updateFields;
                if (string.IsNullOrEmpty(fields))
                {
                    updateFields = prFields.Where(n => n.Name != key.Name).ToList();
                }
                else
                {
                    updateFields = new List<PropertyInfo>();
                    foreach(var item in fields.Split(','))
                    {
                        foreach(var pr in prFields)
                        {
                            if(pr.Name.Equals(item, StringComparison.OrdinalIgnoreCase))
                            {
                                updateFields.Add(pr);
                            }
                        }
                    }
                }
                _updateCommands[type][fields] = $"Update {type.Name} set {string.Join(", ", updateFields.Select(n => $"{n.Name} = @{n.Name}"))} where {key.Name} = @{key.Name};";
            }
            return _updateCommands[type][fields];
        }
    }
}