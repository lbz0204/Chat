using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class ArticleAndMessage
    {
        public Article article { get; set; }
        public List<Agree> agree { get; set; }
        public List<Message> message { get; set; }
    }
}