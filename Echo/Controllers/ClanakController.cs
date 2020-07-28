using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Echo.Controllers
{
    public class ClanakController : Controller
    {
        public IActionResult Index(int? ID)
        {
            if (ID == null)
                return View("Error", new String("Story not found!"));
            AR.ARNews.Clanak c = new AR.ARNews.Clanak((int)ID, null);

            if (c.Status == AR.ARNews.ClanakStatus.Draft)
                if (!Networking.isAdmin(Request))
                    return Redirect("/ControlPanel");

            return View(c);
        }
    }
}