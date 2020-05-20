using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Echo.Models
{
    public class Media
    {
        public IFormFile file { get; set; }
        public object Tag { get; set; }
    }
}
