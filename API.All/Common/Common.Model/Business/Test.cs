using Common.Constant.Enums;
using Common.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Common.Model.Business
{
    public class Test : IRecordState
    {
        public int TestID { get; set; }
        public string TestName { get; set; }
        [NotMapped]
        public ModelState EntityState { get; set; }
    }
}
