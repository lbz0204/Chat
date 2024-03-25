using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Models;

namespace test.ViewModels
{
    public class FriendAndCon
    {
        public string Account { get; set; }
        public string Friend { get; set; }

        public List<Conversation> Con { get; set; }
    }
}