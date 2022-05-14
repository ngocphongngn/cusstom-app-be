using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.Config
{
    public class CenterConfig : ICloneable
    {
        public string Env { get; set; }
        public string[] Cors { get; set; }
        public ConnectionStrings Connections { get; set; }
        public Dictionary<string, string> ApiUrl { get; set; }
        //public FileConfig File { get; set; }
        public object Clone()
        {
            throw (Exception)base.MemberwiseClone();
        }
    }

    public class SwaggerConfig
    {
        public string JsonPath { get; set; }
    }
}
