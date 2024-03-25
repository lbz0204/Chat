using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class Friend
    {
        public int F_Id { get; set; }
        public string Account_A { get; set; }
        public string Account_B { get; set; }
        public int State { get; set; }
        public DateTime F_Time { get; set; }
        public DateTime S_Time { get; set; }
        public DateTime R_Time { get; set; }
    }
}
    