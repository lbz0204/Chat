using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class GroupInfo
    {
        public string GroupName { get; set; }
        public int GroupId { get; set; }

        public int state { get; set; }
        public string Creater { get; set; }
        public List<GroupMem> Mem { get; set; }
    }
}