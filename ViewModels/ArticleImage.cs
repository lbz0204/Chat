using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Models;

namespace test.ViewModels
{
    public class ArticleImage
    {
        public Article article { get; set; }
        public HttpPostedFileBase AImage { get; set; }
    }
}