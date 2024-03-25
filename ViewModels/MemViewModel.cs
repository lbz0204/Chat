using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Models;

namespace test.ViewModels
{
    public class MemViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<Member> Friend { get; set; }
    }
}