using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Models;

namespace test.ViewModels
{
    public class ItemCreateViewModel
    {
        public HttpPostedFileBase ItemImage { get; set; }
        public Item NewData { get; set; }
    }
}