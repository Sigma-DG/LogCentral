using System;
using System.Collections.Generic;
using System.Text;

namespace LogCentral.Common
{
    public class User
    {
        public User()
        {
            RegisterationUtcDate = DateTime.UtcNow;
        }
        public string Username { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisterationUtcDate { get; set; }
        public DateTime LastActivity { get; set; }
        public string Descriptions { get; set; }
    }
}
