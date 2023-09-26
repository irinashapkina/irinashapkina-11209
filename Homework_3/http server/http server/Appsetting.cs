using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace http_server
{
    public class Appsetting
    {
        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("static_files_path")]
        public string StaticPathFiles { get; set; }
    }
}
