using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
        public bool Status { get; set; } = false;
    }
}