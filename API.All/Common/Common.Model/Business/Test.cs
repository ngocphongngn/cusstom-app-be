using Common.Constant.Enums;
using Common.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Common.Model.Attributes;

namespace Common.Model.Business
{
    public class Test : IRecordState
    {
        [Key]
        public int TestID { get; set; }
        public string TestName { get; set; }
        [NotMapped]
        public ModelState EntityState { get; set; }
    }
}
