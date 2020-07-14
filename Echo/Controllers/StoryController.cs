using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Echo.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

namespace Echo.Controllers
{
    public class StoryController : Controller
    {
        public IActionResult Index(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");
            return View(new ClanakModel(ID));
        }

        public IActionResult Edit(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            return Redirect("http://limitlesssoft.com/Dev");
            return View(new AR.ARNews.Clanak(ID));
        }

        public IActionResult New()
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");
            return Redirect("http://limitlesssoft.com/Dev");
            return View(new AR.ARNews.Clanak());
        }

        public IActionResult ADelete(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");
            try
            {
                AR.ARNews.Clanak.Remove(ID);
                return Redirect("/");
            }
            catch(AR.ARException ex)
            {
                return View("Error", "Error occured while deleting story!");
            }
            return View("Error", new String("Unhandled error!"));
        }
        [HttpPost]
        public IActionResult CreateNew([FromBody]AR.ARNews.Clanak c)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");
            if (string.IsNullOrWhiteSpace(c.Naslov))
                return Json("Title mustn't be empty!");

            if(c.Naslov.Length > 256)
                return Json("Title mustn't be longer than 64 characters!");

            c.KorisnikID = Convert.ToInt32(Request.Cookies["kid"]);

            if(c.KorisnikID == null || c.KorisnikID < 1)
                return Json("Error authentication!");

            ClanakModel Clanak = new ClanakModel();
            Clanak.Naslov = c.Naslov;
            Clanak.GrupaID = c.GrupaID;
            Clanak.Tekst = c.Tekst;
            Clanak.KorisnikID = c.KorisnikID;
            Clanak.Slika = c.Slika;
            Clanak.Status = AR.ARNews.ClanakStatus.Published;

            if (Clanak.Kreiraj())
            {
                return Json("success-" + ClanakModel.GetMaxID());
            }
            else
            {
                return Json("Error");
            }
        }
        [HttpPost]
        public IActionResult Update([FromBody]AR.ARNews.Clanak c)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");
            if (string.IsNullOrWhiteSpace(c.Naslov))
                return Json("Title mustn't be empty!");

            if (c.Naslov.Length > 256)
                return Json("Title mustn't be longer than 64 characters!");

            c.KorisnikID = Convert.ToInt32(Request.Cookies["kid"]);

            if (c.KorisnikID == null || c.KorisnikID < 1)
                return Json("Error authentication!");

            try
            {
                using(MySqlConnection con = new MySqlConnection(Security.ConnectionString))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(@"UPDATE CLANAK SET NASLOV = @N,
                        INFO = @I, SLIKA = @S, TEKST = @T, GRUPAID = @GID WHERE CLANAKID = @C", con))
                    {
                        cmd.Parameters.AddWithValue("@N", c.Naslov);
                        cmd.Parameters.AddWithValue("@I", c.Info);
                        cmd.Parameters.AddWithValue("@S", c.Slika);
                        cmd.Parameters.AddWithValue("@T", c.Tekst);
                        cmd.Parameters.AddWithValue("@GID", c.GrupaID);
                        cmd.Parameters.AddWithValue("@C", c.ClanakID);

                        cmd.ExecuteNonQuery();

                        return Json("Story succesfully updated!");
                    }
                }
            }
            catch(Exception ex)
            {
                return Json("Error");
            }
        }
        [HttpPost]
        public IActionResult pUpdate(AR.ARNews.Clanak c)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            try
            {
                c.Update();
                return Redirect("/Story/Edit?ID=" + c.ClanakID);
            }
            catch(Exception ex)
            {
                return View("Error", ex.ToString());
            }
        }
        [HttpPost]
        public IActionResult pCreate(AR.ARNews.Clanak c)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            try
            {
                c.KorisnikID = Networking.GetID(Request);
                AR.ARNews.Clanak.Add(c);
                return Redirect("/Clanak?ID=" + AR.ARNews.Clanak.MaxID());
            }
            catch (Exception ex)
            {
                return View("Error", ex.ToString());
            }
        }

        public IActionResult AUpdateDate(int ID, DateTime Date)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            try
            {
                AR.ARNews.Clanak.SetDate(ID, Date);
                return Json("success");
            }
            catch (Exception ex)
            {
                return Json("ERROR");
            }
        }

        public static void _AMarkVisit(int StoryID)
        {
            Thread t1 = new Thread(() => {
                AR.ARNews.Clanak.RaiseVisit(StoryID);
            });

            t1.Start();
        }
    }
}