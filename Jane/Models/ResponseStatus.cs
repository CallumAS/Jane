using Jane.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jane.Models
{
    public class ResponseStatus
    {
        public Status Status { get; set; }
        public Dictionary<string, string> Variables { get; set; }
        public string Url { get; set; }
    }
}
