using System;
using System.Collections.Generic;
using System.Text;
using Common.Model.Base;
using Common.Constant.Enums;

namespace Common.Model.Parameter
{
    /// <summary>
    /// Tham số để cất
    /// </summary>
    public class SaveParameter<TEntity> : ActionParameterBase
    {
        public SaveMode Mode { get; set; }
        public TEntity Entity { get; set; }
        public TEntity oldEntity { get; set; }
    }
}
