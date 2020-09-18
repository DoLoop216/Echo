using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Echo.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO.Enumeration;
using SixLabors.ImageSharp.ColorSpaces;

namespace Echo.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _env;

        private static string KontaktDir = "Kontakt";
        private static string Top500Dir = "Top500Kontakt";
        private static string AboDir = "ABO";

        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index(int? showFilter, int? onlySalzburg, int? cid)
        {
            return View(new ArchiveFilter(showFilter, onlySalzburg, cid));
        }
        public IActionResult Archieve(int? grupaid)
        {
            return View(grupaid);
        }
        public IActionResult ThePublisher()
        {
            return View("ThePublisher");
        }
        public IActionResult Products()
        {
            return View("Products");
        }
        public IActionResult MediaData()
        {
            return View("MediaData");
        }
        public IActionResult Pricelist()
        {
            return View();
        }
        public IActionResult TOP500Form()
        {
            return View();
        }
        public IActionResult Error(string msg)
        {
            return View("Error", new String(msg));
        }
        public IActionResult Success(string msg)
        {
            return View("Success", new String(msg));
        }
        public IActionResult Contact()
        {
            return View("Contact");
        }
        public IActionResult Subscription()
        {
            return View("Subscription");
        }
        public IActionResult Top500()
        {
            return View();
        }
        public IActionResult News()
        {
            return View();
        }
        public IActionResult Cookies()
        {
            return View();
        }
        public IActionResult AdvertiseHere()
        {
            return View();
        }
        public IActionResult Agb()
        {
            return View();
        }

        public async Task<IActionResult> ASubmitTop500Form(string d1, string d2, string d3, string d4, string d5, string d6, string d7, string d8, string d9,
                string d10, string d11, string d12, string d13, string d14, string d15, string d16, string d17, string d18, string d19, string d20,
                string d21, string d22, string d23, string d24, string d25, string d26, string d27, string d28, string d29, string d30, string d31,
                string d32, string d33, string d34, string d35)
        {

            try
            {
                if (!Directory.Exists(Top500Dir))
                    Directory.CreateDirectory(Top500Dir);

                int i = 1;

                while (System.IO.File.Exists(Path.Combine(Top500Dir, DateTime.Now.ToString("yyyyMMdd") + i.ToString())))
                    i++;

                System.IO.File.WriteAllText(Path.Combine(Top500Dir, DateTime.Now.ToString("yyyyMMdd") + i.ToString()), String.Format(@"<div>
	                    <label style='color: gray'>Firmenname: <span style='color: black;'>{0}</span></label><br>
	                    <label style='color: gray'>Strasse: <span style='color: black;'>{1}</span></label><br>
	                    <label style='color: gray'>Hausnr: <span style='color: black;'>{2}</span></label><br>
	                    <label style='color: gray'>PLZ: <span style='color: black;'>{3}</span></label><br>
	                    <label style='color: gray'>Ort: <span style='color: black;'>{4}</span></label><br>
	                    <label style='color: gray'>Tel.Nr: <span style='color: black;'>{5}</span></label><br>
	                    <label style='color: gray'>Fax.Nr: <span style='color: black;'>{6}</span></label><br>
	                    <label style='color: gray'>Branchentext 1: <span style='color: black;'>{7}</span></label><br>
	                    <label style='color: gray'>Umsatz 2019: <span style='color: black;'>{8}</span></label><br>
	                    <label style='color: gray'>kons. Umsatz: <span style='color: black;'>{9}</span></label><br>
	                    <label style='color: gray'>Gruppenumsatz: <span style='color: black;'>{10}</span></label><br>
	                    <label style='color: gray'>Eigentümer: <span style='color: black;'>{11}</span></label><br>
	                    <label style='color: gray'>Exportanteil in %: <span style='color: black;'>{12}</span></label><br>
	                    <label style='color: gray'>Beschäftigte (Schnitt 18): <span style='color: black;'>{13}</span></label><br>
	                    <label style='color: gray'>Vorstand / GF: <span style='color: black;'>{14}</span></label><br>
	                    <label style='color: gray'>Titel: <span style='color: black;'>{15}</span></label><br>
	                    <label style='color: gray'>Anrede: <span style='color: black;'>{16}</span></label><br>
	                    <label style='color: gray'>Vorname: <span style='color: black;'>{17}</span></label><br>
	                    <label style='color: gray'>Familienname: <span style='color: black;'>{18}</span></label><br>
	                    <label style='color: gray'>Vorstände - Eigentümer: <span style='color: black;'>{19}</span></label><br>
	                    <label style='color: gray'>Leitung Personal Anrede: <span style='color: black;'>{20}</span></label><br>
	                    <label style='color: gray'>Titel: <span style='color: black;'>{21}</span></label><br>
	                    <label style='color: gray'>Vorname: <span style='color: black;'>{22}</span></label><br>
	                    <label style='color: gray'>Familienname: <span style='color: black;'>{23}</span></label><br>
	                    <label style='color: gray'>Pressesprecher: <span style='color: black;'>{24}</span></label><br>
	                    <label style='color: gray'>Marketingleiter: <span style='color: black;'>{25}</span></label><br>
	                    <label style='color: gray'>ANMERKUNGEN zu den obenstehenden Daten: <span style='color: black;'>{26}</span></label><br>
	                    <label style='color: gray'>Meilensteine, Neuerungen, wichtige Ereignisse oder Entwicklungen in Ihrem Unternehmen: <span style='color: black;'>{27}</span></label><br>
	                    <label style='color: gray'>Wie wird die Entwicklung in Ihrem Unternehmen im heurigen Jahr sein: <span style='color: black;'>{28}</span></label><br>
	                    <label style='color: gray'>Haben Sie im heurigen Jahr Mitarbeiter abgebaut, ist der Personalstand gestiegen oder ist er gleichgeblieben: <span style='color: black;'>{29}</span></label><br>
	                    <label style='color: gray'>Planen Sie in den nächsten 12 Monaten: <span style='color: black;'>{30}</span></label><br>
	                    <label style='color: gray'>Bilden Sie Lehrlinge aus: <span style='color: black;'>{31}</span></label><br>
	                    <label style='color: gray'>Wieviel Lehrlinge pro Jahr: <span style='color: black;'>{32}</span></label><br>
	                    <label style='color: gray'>In welchen Berufen werden Lehrlinge in Ihrem Unternehmen ausgebildet: <span style='color: black;'>{33}</span></label><br>
	                    <label style='color: gray'>Bitte nennen Sie uns das Gründungsjahr Ihres Unternehmens: <span style='color: black;'>{34}</span></label><br>
                    </div>", d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12, d13, d14, d15, d16, d17, d18, d19, d20, d21, d22, d23, d24, d25, d26, d27, d28,
                    d29, d30, d31, d32, d33, d34, d35));
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }

            return Json("success");
        }


        public async Task<IActionResult> AUploadImage1(IList<IFormFile> files)
        {
            string otp = "";
            foreach (IFormFile source in files)
            {
                string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');

                filename = this.EnsureCorrectFilename(filename);

                using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename)))
                    await source.CopyToAsync(output);

                otp += "<img class='GalleryItem' src='\\images\\" + filename + "' />";
            }

            return Json(otp);
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            return this._env.WebRootPath + "\\images\\" + filename;
        }
        
        public IActionResult AUploadImage(Media m)
        {
            try
            {
                string folderPath = _env.WebRootPath + "\\images";
                string fileName = Security.HashPassword(Program.rnd.Next(0, 10000).ToString());



                while(System.IO.File.Exists(folderPath + "\\" + fileName))
                    fileName = Security.HashPassword(Program.rnd.Next(0, 10000).ToString());


                using (System.IO.FileStream fs = new System.IO.FileStream(folderPath + "\\" + fileName + ".jpg", FileMode.Create))
                    m.file.CopyTo(fs);


                return Json("Successfully uploaded image!");
            }
            catch(Exception ex)
            {
                return Json("Error");
            }
        }
        public async Task<IActionResult> ASendContact(string firm, string anrede, string name,
            string zus, string ul,  string nr, string plz, string stad,
            string bund, string land, string em, string nach)
        {
            try
            {
                if (!Directory.Exists(KontaktDir))
                    Directory.CreateDirectory(KontaktDir);

                int i = 1;

                while (System.IO.File.Exists(Path.Combine(KontaktDir, DateTime.Now.ToString("yyyyMMdd") + i.ToString())))
                    i++;


                System.IO.File.WriteAllText(Path.Combine(KontaktDir, DateTime.Now.ToString("yyyyMMdd") + i.ToString()), String.Format(@"<p>Firma: {0}</p><p>Anrede: {1}</p>
                        <p>Name: {2}</p><p>Zusatz: {3}</p><p>Strase: {4}</p><p>Nr: {5}</p>
                        <p>PLZ: {6}</p><p>Stadt: {7}</p><p>Bundersland: {8}</p><p>Land: {9}</p>
                        <p>Email: {10}</p><p>Nachricht: {11}</p>",
                        firm, anrede, name, zus, ul, nr, plz, stad, bund, land, em, nach));
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }

            return Json("success");
        }

        public async Task<IActionResult> ASendAbo(int period, string Name, string Strase, string Nr,
            string Plz, string Ort, string Land, string Telefon, string email)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(AboDir))
                        Directory.CreateDirectory(AboDir);

                    int i = 1;

                    while (System.IO.File.Exists(Path.Combine(AboDir, DateTime.Now.ToString("yyyyMMdd") + i.ToString())))
                        i++;


                    System.IO.File.WriteAllText(Path.Combine(AboDir, DateTime.Now.ToString("yyyyMMdd") + i.ToString()), $"<p>Period: { (period == 0 ? "3 Monate kostenlos" : "14 Ausgaben um nur € 28,--")}</p><p>Name: {Name}</p>" +
                        $"<p>Straße: {Strase}</p><p>Nr.: {Nr}</p><p>PLZ: {Plz}</p><p>Ort: {Ort}</p>" +
                        $"<p>Land: {Land}</p><p>Telefon: {Telefon}</p><p>Email: {email}</p>");
                }
                catch (Exception ex)
                {
                    return Json(ex.ToString());
                }

                return Json("success");
            });
        }

        public static List<string> GetKontaktFiles()
        {
            if (!System.IO.Directory.Exists(KontaktDir))
                return null;
            List<string> list = new List<string>();

            foreach(string f in Directory.GetFiles(KontaktDir))
                list.Add(f);

            return list;
        }
        public static List<string> GetTop500Files()
        {
            if (!System.IO.Directory.Exists(Top500Dir))
                return null;

            List<string> list = new List<string>();

            foreach (string f in Directory.GetFiles(Top500Dir))
                list.Add(f);

            return list;
        }
        public static List<string> GetAboFiles()
        {
            if (!System.IO.Directory.Exists(AboDir))
                return null;

            List<string> list = new List<string>();

            foreach (string f in Directory.GetFiles(AboDir))
                list.Add(f);

            return list;
        }

        [Route("/Home/Kontakt/{fileName}")]
        public IActionResult Kontakt(string fileName)
        {
            if (!System.IO.Directory.Exists(KontaktDir))
                return View("Error", "File doesn't exist!");

            if (!System.IO.File.Exists(Path.Combine(KontaktDir, fileName)))
                return View("Error", "File doesn't exist!");

            string cont = System.IO.File.ReadAllText(Path.Combine(KontaktDir, fileName));

            return View("Kontakt", cont);
        }


        [Route("/Home/Top500Details/{fileName}")]
        public IActionResult Top500Details(string fileName)
        {
            if(!System.IO.Directory.Exists(Top500Dir))
                return View("Error", "File doesn't exist!");

            if (!System.IO.File.Exists(Path.Combine(Top500Dir, fileName)))
                return View("Error", "File doesn't exist!");

            string cont = System.IO.File.ReadAllText(Path.Combine(Top500Dir, fileName));

            return View("Top500Details", cont);
        }

        [Route("/Home/ABODetails/{fileName}")]
        public IActionResult ABODetails(string fileName)
        {
            if (!System.IO.Directory.Exists(AboDir))
                return View("Error", "File doesn't exist!");

            if (!System.IO.File.Exists(Path.Combine(AboDir, fileName)))
                return View("Error", "File doesn't exist!");

            string cont = System.IO.File.ReadAllText(Path.Combine(AboDir, fileName));

            return View("ABODetails", cont);
        }
    }
}
