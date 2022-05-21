using Common.Model.Attributes;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.Utility.Services
{
    public class TypeService : ITypeService
    {
        /// <summary>
        /// Tạo danh sách theo type
        /// </summary>
        public IList CreateList(Type type)
        {
            Type listType = typeof(List<>).MakeGenericType(new[] { type });
            IList list = (IList)Activator.CreateInstance(listType);
            return list;
        }
        /// <summary>
        /// Lấy kiểu dữ liệu trong list
        /// </summary>
        public Type GetTypeInList(Type listType)
        {
            if (IsList(listType))
            {
                Type type = listType.GetGenericArguments()[0];
                return type;
            }
            return null;
        }
        /// <summary>
        /// Check có phải list không?
        /// </summary>
        public bool IsList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        private ConcurrentDictionary<string, List<PropertyInfo>> _entityProperties = new ConcurrentDictionary<string, List<PropertyInfo>>();
        /// <summary>
        /// Lấy thông tin property của class model
        /// </summary>
        public List<PropertyInfo> GetProperties(Type type)
        {
            if (type == null)
            {
                return null;
            }
            var typeKey = type.FullName;
            if (!_entityProperties.ContainsKey(typeKey))
            {
                _entityProperties[typeKey] = type.GetProperties().ToList();
            }
            return _entityProperties[typeKey];
        }
        /// <summary>
        /// Lấy thông tin property của class model
        /// </summary>
        public List<PropertyInfo> GetProperties<T>()
        {
            var type = typeof(T);
            return GetProperties(typeof(T));
        }
        /// <summary>
        /// Lấy thuộc tính khóa của dữ liệu
        /// </summary>
        public PropertyInfo GetKeyProperty(Type type)
        {
            var attrs = GetPropertys<KeyAttribute>(type);
            if (attrs.Count > 0)
            {
                return attrs.First().Key;
            }
            return null;
        }
        private ConcurrentDictionary<string, Dictionary<string, Dictionary<PropertyInfo, Attribute>>> _modelAttributes = new ConcurrentDictionary<string, Dictionary<string, Dictionary<PropertyInfo, Attribute>>>();
        /// <summary>
        /// Lấy danh sách các thuộc tính theo attribute
        /// </summary>
        public Dictionary<PropertyInfo, Attribute> GetPropertys<TAttribute>(Type entityType) where TAttribute : Attribute
        {
            if (entityType == null)
            {
                return null;
            }
            var entityTypeKey = entityType.FullName;
            if (!_modelAttributes.ContainsKey(entityTypeKey))
            {
                _modelAttributes[entityTypeKey] = new Dictionary<string, Dictionary<PropertyInfo, Attribute>>();
            }
            var attrType = typeof(TAttribute);
            if (attrType == null)
            {
                return null;
            }
            var attrTypeKey = attrType.FullName;
            var attrs = _modelAttributes[entityTypeKey];
            if (!attrs.ContainsKey(attrTypeKey))
            {
                var result = new Dictionary<PropertyInfo, Attribute>();
                var prs = GetProperties(entityType);
                foreach (var pr in prs)
                {
                    var attr = pr.GetCustomAttribute<TAttribute>();
                    if (attr != null)
                    {
                        result.Add(pr, attr);
                    }
                }
                attrs[attrTypeKey] = result;
            }
            return attrs[attrTypeKey];
        }
        // <summary>
        /// Lấy thông tin theo tên thuộc tính
        /// </summary>
        public PropertyInfo GetProperty(Type type, string propertyName)
        {
            var prs = GetProperties(type);
            return prs.FirstOrDefault(n => n.Name.Equals(propertyName));
        }
        // <summary>
        /// Lấy thông tin theo tên thuộc tính
        /// </summary>
        public PropertyInfo GetProperty<T>(string propertyName)
        {
            return GetProperty(typeof(T), propertyName);
        }
        private ConcurrentDictionary<string, List<PropertyInfo>> _tableProperties = new ConcurrentDictionary<string, List<PropertyInfo>>();
        /// <summary>
        /// Lấy thuộc tính mapping với các trường dữ liệu trogn db
        /// </summary>
        public List<PropertyInfo> GetTableProperty(Type type)
        {
            if (type == null)
            {
                return null;
            }
            var typeKey = type.FullName;
            if (!_tableProperties.ContainsKey(typeKey))
            {
                var prs = GetProperties(type);
                var fields = new List<PropertyInfo>();
                foreach (var item in prs)
                {
                    if (item.GetCustomAttribute<NotMappedAttribute>() == null)
                    {
                        fields.Add(item);
                    }
                }
                _tableProperties[typeKey] = fields;
            }
            return _tableProperties[typeKey];
        }
        /// <summary>
        /// Check xem thuộc tính có trong bảng không?
        /// </summary>
        public PropertyInfo GetTableProperty(Type type, string propertyName)
        {
            var prs = this.GetTableProperty(type);
            return prs.FirstOrDefault(n => n.Name == propertyName);
        }
    }
}