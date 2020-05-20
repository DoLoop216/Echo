using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Echo.Models
{
    public class NewsModel
    {
        public AR.ARNews.News News { get; set; }
        public IFormFile Thumbnail { get; set; }
        public IFormFile Media { get; set; }
    }
}
