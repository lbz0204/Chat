using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Models;

namespace test.ViewModels
{
    public class ArticleAndFriend
    {
        public ArticleAndMessage Art { get; set; }
        public List<Friend> Fri { get; set; }
    }
}