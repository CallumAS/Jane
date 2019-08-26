using System;
using System.Collections.Generic;
using System.Text;

namespace Jane.Models
{
    public class Config
    {
        public General General { get; set; }
        public List<Request> Requests { get; set; }
    }
}
