using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class ImageNew
    {
        public int Id { get; set; }
        public string ImageN { get; set; }
        public string Image { get; set; }
        public int Type { get; set; }
        public DateTime Time { get; set; }

        public string Source { get; set; }
        public string Description { get; set; }
    }
    
}