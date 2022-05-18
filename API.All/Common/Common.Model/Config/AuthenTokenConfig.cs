using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.Config
{
    public class AuthenTokenConfig
    {
        public string JwtSecretKey { get; set; }
        public string JwtIssuer { get; set; }
        public double ExpiredHours { get; set; }
        public double TempCacheMinutes { get; set; }
    }
}
