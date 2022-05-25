using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.Attributes
{
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// tên bảng chi tiết
        /// </summary>
        public string Table { get; set; }
        public TableAttribute(string table)
        {
            this.Table = table;
        }
    }
}
