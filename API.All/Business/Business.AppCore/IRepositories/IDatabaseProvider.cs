using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Business.AppCore.IRepositories
{
    public interface IDatabaseProvider
    {
        /// <summary>
        /// Lấy chuỗi kết nối
        /// </summary>
        string GetConnectionString();
        /// <summary>
        /// Lấy kết nối
        /// </summary>
        IDbConnection GetConnection();
        /// <summary>
        /// Lấy kêt nối vả open lun
        /// </summary>
        IDbConnection GetOpenConnection();
        /// <summary>
        /// Giải phóng connection
        /// </summary>
        void CloseConnection(IDbConnection connection);
        /// <summary>
        /// Thủ tục trả về danh sách dữ liệu
        /// </summary>
        List<T> ExecuteQueryObject<T>(string storeName, object param = null);
        /// <summary>
        /// Thủ tục trả về danh sách dữ liệu
        /// </summary>
        List<T> ExecuteQueryObject<T>(IDbConnection cnn, string storeName, object param = null);
        /// <summary>
        /// Thủ tục trả về row effect
        /// </summary>
        int ExecuteNonQueryText(string commandText, Dictionary<string, object> param, int? timeout = null);
        /// <summary>
        /// Thủ tục trả về row effect
        /// </summary>
        int ExecuteNonQueryText(IDbConnection cnn, string commandText, Dictionary<string, object> param, int? timeout = null);
        /// <summary>
        /// Thủ tục trả về row effect
        /// </summary>
        int ExecuteNonQueryText(IDbTransaction transaction, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thủ tục trả về row effect
        /// </summary>
        int ExecuteNonQueryText(string commandText, object param);
        /// <summary>
        /// Thủ tục trả về row effect
        /// </summary>
        int ExecuteNonQueryText(IDbConnection cnn, string commandText, object param);
        /// <summary>
        /// Thủ tục trả về row effect
        /// </summary>
        int ExecuteNonQueryText(IDbTransaction transaction, string commandText, object param);
        /// <summary>
        /// Thực hiện sql trả về cell đầu tiên
        /// </summary>
        object ExecuteScalarText(string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về cell đầu tiên
        /// </summary>
        object ExecuteScalarText(IDbConnection cnn, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về cell đầu tiên
        /// </summary>
        object ExecuteScalarText(IDbTransaction transaction, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về cell đầu tiên
        /// </summary>
        object ExecuteScalarText(string commandText, object param);
        /// <summary>
        /// Thực hiện sql trả về cell đầu tiên
        /// </summary>
        object ExecuteScalarText(IDbConnection cnn, string commandText, object param);
        /// <summary>
        /// Thực hiện sql trả về cell đầu tiên
        /// </summary>
        object ExecuteScalarText(IDbTransaction transaction, string commandText, object param);
        /// <summary>
        /// Thực hiện sql trả về danh sách
        /// </summary>
        List<T> Query<T>(string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về danh sách
        /// </summary>
        List<T> Query<T>(IDbConnection cnn, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về danh sách
        /// </summary>
        List<T> Query<T>(IDbTransaction transaction, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về danh sách
        /// </summary>
        IList Query(Type type, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về danh sách
        /// </summary>
        IList Query(IDbConnection cnn, Type type, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về danh sách
        /// </summary>
        IList Query(IDbTransaction transaction, Type type, string commandText, Dictionary<string, object> param);

        /// <summary>
        /// Thực hiện sql trả về danh sách
        /// </summary>
        IEnumerable<dynamic> Query(string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về danh sách
        /// </summary>
        IEnumerable<dynamic> Query(IDbConnection cnn, string commandText, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về danh sách dữ liệu vs thủ tục hoặc là text
        /// </summary>
        List<T> QueryWithCommandType<T>(IDbConnection cnn, string commandText, CommandType pCommandType, Dictionary<string, object> param);
        /// <summary>
        /// Thực hiện sql trả về danh sách dữ liệu vs thủ tục hoặc là text
        /// </summary>
        List<T> QueryWithCommandType<T>(IDbTransaction transaction, string commandText, CommandType pCommandType, Dictionary<string, object> param);
    }
}
