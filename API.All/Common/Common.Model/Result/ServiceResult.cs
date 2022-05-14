using Common.Constant.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.Result
{
    public class ServiceResult
    {
        public bool Success { get; set; } = true;
        public ServiceResponseCode Code { get; set; } = ServiceResponseCode.Success;
        public string SubCode { get; set; }
        public string UserMessage { get; set; }
        public string SystemMessage { get; set; }
        public object Data { get; set; }
        public ServiceResult() { }
        public override string ToString()
        {
            if (Success)
            {
                return $"Success";
            }
            else
            {
                return $"Failed - code: {Code}-{SubCode} - SysMessage: {SystemMessage} - UserMessage: {UserMessage}";
            }
        }
        public ServiceResult OnSuccess(object data = null)
        {
            this.Data = data;
            return this;
        }

        public ServiceResult OnException(Exception ex)
        {
            if(ex != null)
            {
                this.Success = false;
                this.Code = ServiceResponseCode.Error;
#if true
                this.SystemMessage = ex.Message;
                if(ex.InnerException != null)
                {
                    this.SystemMessage += Environment.NewLine + ex.InnerException.Message;
                }
#else
                this.SystemMessage = "Exception."
#endif
                this.SystemMessage = ex.ToString();
            }
            return this;
        }

        public ServiceResult OnError(ServiceResponseCode code, string subCode = "", object data = null, string userMessage = "", string systemMessage = "")
        {
            this.Success = false;
            this.Code = code;
            this.SubCode = subCode;
            this.Data = data;
            this.SystemMessage = systemMessage;
            this.UserMessage = userMessage;
            if (string.IsNullOrEmpty(systemMessage))
            {
                this.SystemMessage = code + "";
            }
            return this;
        }
    }
}
