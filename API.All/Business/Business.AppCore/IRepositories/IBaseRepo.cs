using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Common.Model.Base;
using Common.Model.Other;

namespace Business.AppCore.IRepositories
{
    public interface IBaseRepo
    {
        /// <summary>
        /// Đối tượng thao tác với db
        /// </summary>
        IDatabaseProvider Provider { get; }
        void setReturnNewIdWhenInsert(bool value);
        /// <summary>
        /// Get dữ liệu từ bảng theo 1 field
        /// </summary>
        List<T> Get<T>(string field, object value, string op = "=");
        /// <summary>
        /// Get dữ liệu từ bảng theo 1 field
        /// </summary>
        List<T> Get<T>(IDbConnection cnn, string field, object value, string op = "=", string columns = "*");
        /// <summary>
        /// Get dữ liệu từ bảng theo 1 field
        /// </summary>
        List<T> Get<T>(IDbTransaction transaction, string field, object value, string op = "=");
        /// <summary>
        /// Lấy kiểu dữ liệu trả về
        /// </summary>
        List<T> Get<T>();
        /// <summary>
        /// Lấy kiểu dữ liệu trả về
        /// </summary>
        List<T> Get<T>(IDbConnection cnn);
        /// <summary>
        /// Lấy kiểu dữ liệu theo id
        /// </summary>
        T GetById<T>(object id);
        /// <summary>
        /// Lấy kiểu dữ liệu theo id
        /// </summary>
        T GetById<T>(IDbConnection cnn, object id);
        /// <summary>
        /// Tùy biến build câu query
        /// </summary>
        string buildQueryById<T>();
        /// <summary>
        /// Thêm dữ liệu
        /// </summary>
        object Insert(object entity);
        /// <summary>
        /// Thêm dữ liệu
        /// </summary>
        object Insert(IDbConnection cnn, object entity);
        /// <summary>
        /// Thêm dữ liệu
        /// </summary>
        object Insert(IDbTransaction transaction, object entity);
        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        bool Update(object entity, string fields = null);
        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        bool Update(IDbConnection cnn, object entity, string fields = null);
        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        bool Update(IDbTransaction transaction, object entity, string fields = null);
        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        bool Delete(object entity);
        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        bool Delete(IDbConnection cnn, object entity);
        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        bool Delete(IDbTransaction transaction, object entity);
        /// <summary>
        /// Submit lst dữ liệu
        /// </summary>
        void Submit(List<SubmitModel> data);
        /// <summary>
        /// Submit lst dữ liệu
        /// </summary>
        void Submit(IDbConnection cnn, List<SubmitModel> data);
        /// <summary>
        /// Submit lst dữ liệu
        /// </summary>
        void Submit(IDbTransaction transaction, List<SubmitModel> data);

    }
}