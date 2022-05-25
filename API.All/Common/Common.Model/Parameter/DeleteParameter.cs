using Common.Constant.Enums;
using Common.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.Parameter
{
    /// <summary>
    /// Tham số hàm xóa
    /// </summary>
    public class DeleteParameter<TEntity> : ActionParameterBase
    {
        /// <summary>
        /// Bản ghi xóa
        /// </summary>
        public TEntity Entity { get; set; }
        /// <summary>
        /// Loại xóa: 1.đơn hay 2.hàng loạt
        /// </summary>
        public DeleteType Type { get; set; }
    }
}
