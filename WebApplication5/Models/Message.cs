using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class Message
    {
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string MessageText { get; set; }
    }
}