using Common.Constant.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.Base
{
    public class ModelData
    {
        /// <summary>
        /// Tên bảng update
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
