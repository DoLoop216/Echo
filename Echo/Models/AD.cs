using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Echo.Models
{
    public class AD
    {
        public IFormFile File { get; set; }
        public string Redirect { get; set; }
        public int Type { get; set; }
    }
}
