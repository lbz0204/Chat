using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class Message
    {
        public int A_Id { get; set; }
        public int M_Id { get; set; }
        public string Account { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
    }
}