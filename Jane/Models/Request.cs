using System;
using System.Collections.Generic;
using System.Text;

namespace Jane.Models
{
    public class Request
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public string UserAgent { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public bool CaptureRaw { get; set; }
        public List<Regex> RegexVariables { get; set; }
        public List<GetBetween> GetbetweenVariables { get; set; }
        public List<string> SuccessKeys { get; set; }
        public List<string> FailureKeys { get; set; }
    }
}
