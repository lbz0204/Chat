using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class Conversation
    {
        public int C_Id { get; set; }
        public string Account_A { get; set; }
        public string Account_B { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
    }
}