using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class User
    {
        public string UserName { get; set; }

        public string ConnectionID { get; set; }

        public bool Connected { get; set; }
    }
}