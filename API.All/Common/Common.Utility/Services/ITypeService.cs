using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Common.Utility.Services
{
    public interface ITypeService
    {
        /// <summary>
        /// Tạo danh sách theo type
        /// </summary>
        IList CreateList(Type type);
        /// <summary>
        /// Có phải list?
        /// </summary>
        bool IsList(Type type);
        /// <summary>
        /// Lấy kiểu dữ liệu trong list
        /// </summary>
        Type GetTypeInList(Type listType);
        /// <summary>
        /// Lấy thông tin property của class model
        /// </summary>
        List<PropertyInfo> GetProperties(Type type);
        /// <summary>
        /// Lấy thông tin property của class model
        /// </summary>
        List<PropertyInfo> GetProperties<T>();
        /// <summary>
        /// Lấy thuộc tính khóa của dữ liệu
        /// </summary>
        PropertyInfo GetKeyProperty(Type type);
        // <summary>
        /// Lấy thông tin theo tên thuộc tính
        /// </summary>
        PropertyInfo GetProperty(Type type, string propertyName);
        // <summary>
        /// Lấy thông tin theo tên thuộc tính
        /// </summary>
        PropertyInfo GetProperty<T>(string propertyName);
        /// <summary>
        /// Lấy thuộc tính mapping với trường dữ liệu lưu trong db
        /// </summary>
        List<PropertyInfo> GetTableProperty(Type type);
        /// <summary>
        /// Lấy thuộc tính mapping với trường dữ liệu lưu trong db
        /// </summary>
        PropertyInfo GetTableProperty(Type type, string propertyName);
        // <summary>
        /// Lấy danh sách các thuộc tính theo attribute
        /// </summary>
        Dictionary<PropertyInfo, Attribute> GetPropertys<TAttribute>(Type entityType) where TAttribute : Attribute;
    }
}
