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

            string isContentUnlocked = Request.Cookies["content_unlocked"];
            AR.ARNews.Clanak c = new AR.ARNews.Clanak((int)ID, null);
            
            if (c.Status == AR.ARNews.ClanakStatus.Draft)
                if (!Networking.isAdmin(Request))
                    return Redirect("/ControlPanel");

            if (!string.IsNullOrWhiteSpace(isContentUnlocked) && isContentUnlocked == "UnlockEcho2021")
                c.Locked = false;
            
            return View(c);
        }

        [HttpPost]
        public IActionResult UnlockContent(string pw)
        {
            if (pw == "UnlockEcho2021")
                Response.Cookies.Append("content_unlocked", "UnlockEcho2021");

            return Ok();
        }
    }
}