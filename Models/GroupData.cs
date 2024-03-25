using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class GroupData
    {
        public string GroupName { get; set; }
        public int Group_Id { get; set; }
        public List<GroupMem> AllMem { get; set; }
        public List<GroupCon> AllCon { get; set; }
    }
}