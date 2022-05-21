using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.Other
{
    public class SqlQuery
    {
        /// <summary>
        /// Số lượng bản ghi
        /// </summary>
        public int RecordCount { get; set; }
        /// <summary>
        /// Nội dung query
        /// </summary>
        public string Query { get; set; }
        /// <summary>
        /// Tham số
        /// </summary>
        public Dictionary<string, object> Param { get; set; }
        /// <summary>
        /// Danh sách bản ghi tạo ra
        /// </summary>
        public IList Records { get; set; }
    }
}
