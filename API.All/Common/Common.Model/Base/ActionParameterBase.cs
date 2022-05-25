using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.Base
{
    public class ActionParameterBase
    {
        /// <summary>
        /// Danh sách các rule validate
        /// </summary>
        public List<string> ByPassValidate { get; set; }
        /// <summary>
        /// Kiểm tra có pas qua các rule validate không?
        /// </summary>
        public bool CheckByPass(string code)
        {
            if(this.ByPassValidate != null && !string.IsNullOrEmpty(code))
            {
                foreach(var item in this.ByPassValidate)
                {
                    if(code.Equals(item, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
