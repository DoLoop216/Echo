using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Echo.Controllers
{
    public class ARController : Controller
    {
        public IActionResult ADDV(string ID = "all", int minviews = 0, int maxviews = 5)
        {
            try
            {
                if (ID == "all")
                {
                    foreach (AR.ARNews.Clanak c in AR.ARNews.Clanak.List(new AR.ARNews.ClanakSettings()))
                        AR.ARNews.Clanak.RaiseVisit(c.ClanakID);
                }
                else
                {
                    AR.ARNews.Clanak.RaiseVisit(Convert.ToInt32(ID));
                }
                return View("Success", "Uspesno!");
            }
            catch(Exception ex)
            {
                return View("Error", ex.ToString());
            }
        }
    }
}