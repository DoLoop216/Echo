using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Echo.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            return View(new AR.ARNews.User(ID, null));
        }

        public IActionResult New()
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            return View(new AR.ARNews.User());
        }

        public IActionResult List()
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            return View();
        }
        public IActionResult ResetPW(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            return View(ID);
        }


        public IActionResult AResetPW(int userID, string pw)
        {
            try
            {
                AR.ARNews.User.SetPassword(userID, Security.HashPassword(pw), null);
                return Json("success");
            }
            catch (Exception ex)
            {
                return Json("error");
            }
        }

        [HttpPost]
        public IActionResult CreateNew(AR.ARNews.User user)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");
            try
            {
                AR.ARNews.User.Add(user, null);
                return Redirect("/User/List");
            }
            catch (Exception ex)
            {
                return View("Error", ex.ToString());
            }
        }
        [HttpPost]
        public IActionResult AUpdate(AR.ARNews.User user)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            user.Update(null);

            return Redirect("/User?ID=" + user.ID);
        }
    }
}