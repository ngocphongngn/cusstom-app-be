using Common.Constant.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.Other
{
    /// <summary>
    /// Đối tượng submit dữ liệu vào db
    /// </summary>
    public class SubmitModel
    {
        /// <summary>
        /// Bảng dữ liệu
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Trạng thái thêm sửa xóa
        /// </summary>
        public ModelState State { get; set; }
        /// <summary>
        /// Dữ liệu
        /// </summary>
        public List<Dictionary<string, object>> Datas { get; set; }
        /// <summary>
        /// Khóa chính
        /// </summary>
        public List<string> KeyFields { get; set; }

    }
}
