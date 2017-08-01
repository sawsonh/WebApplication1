using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Core.Entities
{
    public class User
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Dictionary<string, string> Roles { get; set; }
    }
}
