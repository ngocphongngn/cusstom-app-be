using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.Config
{
    public class RouterConfig
    {
        public string DefaultEnv { get; set; }
        public Dictionary<string, string> VersionMapping { get; set; }
        public Dictionary<string, Dictionary<string, string>> ApiUrl { get; set; }
        public Dictionary<string, List<string>> MappingKey { get; set; }
        public List<string> MappingKeyOrder { get; set; }
    }
}
