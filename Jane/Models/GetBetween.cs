using System;
using System.Collections.Generic;
using System.Text;

namespace Jane.Models
{
    public class GetBetween
    {
        public string Name { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
        public bool IncludeLeft { get; set; }
        public bool IncludeRight { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
    }
}
