using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Echo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using AR;
using System.Threading;
using AR;

namespace Echo.Controllers
{
    public class ControlPanelController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public ControlPanelController(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
        }

        public IActionResult Index()
        {
            string hash = Request.Cookies["h"];
            ARWebAuthorization.User user = ARWebAuthorization.GetUser(hash);

            if (user == null)
                return View(new AR.ARNews.User());
            else
            {
                switch((user.LocalUserClass as AR.ARNews.User).Type)
                {
                    case AR.ARNews.UserType.Administrator:
                        return View("Admin", user.LocalUserClass as AR.ARNews.User);
                    case AR.ARNews.UserType.Subscriber:
                        return View("Subscriber", user.LocalUserClass as AR.ARNews.User);
                    default:
                        return View(new AR.ARNews.User());
                }
            }
        }
        public IActionResult Admin(string modul)
        {
            if (!Networking.isAdmin(Request))
                return View("Index");

            switch(modul)
            {
                case "Users":
                    return View("Users", AR.ARNews.User.List(null));
                case "Stories":
                    return View("Stories", ClanakModel.List(Request));
                case "News":
                    return View("News");
                case "Top500":
                    return View("Top500");
                case "CM":
                    return View("CircuralMessage");
                case "Statistics":
                    return View("Statistics");
                case "Emails":
                    return View("DBMails");
                case "ad":
                    return View("Advertisements");
                default:
                    return View(ARWebAuthorization.GetUser(Request.Cookies["h"]));
            }
        }
        public IActionResult NewNews()
        {
            if (!Networking.isAdmin(Request))
                return View("Index");

            return View(new NewsModel());
        }
        public IActionResult NewEmployee()
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            return View(new AR.ARNews.Employee());
        }
        public IActionResult GroupMng()
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            return View();
        }
        public IActionResult EditEmployee(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            return View(new AR.ARNews.Employee(ID, null));
        }
        public IActionResult Employees()
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");
            
            return View();
        }
        [HttpPost]
        public IActionResult AUpdateGroup(AR.ARNews.Grupa g)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            g.Update(null);

            return Redirect("/ControlPanel/GroupMng");
        }
        public IActionResult ACreateNewGroup(string Name)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            AR.ARNews.Grupa.Add(Name, null);

            return Redirect("/ControlPanel/GroupMng");
        }
        public IActionResult UploadNewAD()
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            return View(new AD());
        }
        public IActionResult UploadNewProduct()
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            return View(new AD());
        }
        public IActionResult EditNonFixedText(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            return View(AR.ARNews.NonFixedText.Get(ID, null));
        }
        public IActionResult Logoff()
        {
            ARWebAuthorization.LogoutUser(Request.Cookies["h"]);
            return Redirect("/Home");
        }
        public IActionResult ARemoveEmployee(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            AR.ARNews.Employee.Remove(ID, null);

            return Redirect("/ControlPanel/Employees");
        }
        [HttpPost]
        public IActionResult AEditEmployee(AR.ARNews.Employee emp)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");


            AR.ARNews.Employee.Update(emp, null);

            return Redirect("/ControlPanel/EditEmployee?ID=" + emp.ID);
        }
        public IActionResult AUploadAD(AD m)
        {
            if (!Networking.isAdmin(Request))
                return Json("Insuffsicient permission!");
            try
            {
                string folderPath = _hostingEnvironment.WebRootPath + "\\ads";
                string fileName = Security.HashPassword(Program.rnd.Next(0, 10000).ToString());

                while (System.IO.File.Exists(folderPath + "\\" + fileName))
                    fileName = Security.HashPassword(Program.rnd.Next(0, 10000).ToString());
                string ext = m.File.FileName.Split('.')[1];
                using (System.IO.FileStream fs = new System.IO.FileStream(folderPath + "\\" + fileName + "." + ext, FileMode.Create))
                    m.File.CopyTo(fs);


                AR.ARNews.Banner.Add(m.Type, "/ads/" + fileName + "." + ext, m.Redirect, null);


                return Redirect("/ControlPanel/Admin?modul=ad");
            }
            catch
            {
                return Json("Error");
            }
        }
        public IActionResult AUploadProduct(AD m)
        {
            if (!Networking.isAdmin(Request))
                return Json("Insuffsicient permission!");
            try
            {
                string folderPath = _hostingEnvironment.WebRootPath + "\\Products";
                string fileName = Security.HashPassword(Program.rnd.Next(0, 10000).ToString());

                while (System.IO.File.Exists(folderPath + "\\" + fileName))
                    fileName = Security.HashPassword(Program.rnd.Next(0, 10000).ToString());

                string ext = m.File.FileName.Split('.')[1];
                switch(m.Type)
                {
                    case 1337:
                        using (System.IO.FileStream fs = new System.IO.FileStream(folderPath + "\\Echo\\" + fileName + "." + ext, FileMode.Create))
                            m.File.CopyTo(fs);
                        AR.ARNews.Banner.Add(m.Type, "/Products/Echo/" + fileName + "." + ext, m.Redirect, null);
                        break;
                    case 1338:
                        using (System.IO.FileStream fs = new System.IO.FileStream(folderPath + "\\Other\\" + fileName + "." + ext, FileMode.Create))
                            m.File.CopyTo(fs);
                        AR.ARNews.Banner.Add(m.Type, "/Products/Other/" + fileName + "." + ext, m.Redirect, null);
                        break;
                    default:
                        throw new Exception("Unknown type!");
                }

                return Redirect("/Home/Products");
            }
            catch(Exception ex)
            {
                return Json("Error");
            }
        }
        public IActionResult ARemoveBanner(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            AR.ARNews.Banner.Remove(ID, null);

            return Redirect("/ControlPanel/Admin?modul=ad");
        }
        public IActionResult ARemoveProduct(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            AR.ARNews.Banner.Remove(ID, null);

            return Redirect("/Home/Products");
        }
        public IActionResult ARemoveMail(int ID, string h)
        {
            if (!Networking.isAdmin(Request) && Security.HashPassword((ID + 1).ToString()) != h)
                return Redirect("/ControlPanel");

            AR.ARNews.Mail.Remove(ID, null);

            if (Networking.isAdmin(Request))
                return Redirect("/ControlPanel/Admin?modul=Emails");
            else
                return View("Success", "You have succesfully removed your email from newsletter!");
        }
        public IActionResult AUpdateNFT(int ID, string TEXT)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            AR.ARNews.NonFixedText.Set(ID, TEXT, null);

            return Json("Success");
        }
        public IActionResult AShowNews(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            AR.ARNews.News.Show(ID, null);

            return Redirect("/Home/News");
        }
        public IActionResult AHideNews(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            AR.ARNews.News.Hide(ID, null);

            return Redirect("/Home/News");
        }
        public IActionResult ADeleteNews(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            AR.ARNews.News.Remove(ID, null);

            return Redirect("/Home/News");
        }
        [HttpPost]
        public IActionResult AAddMultipleMails(string mails)
        {
            if (!Networking.isAdmin(Request))
                return Json("Insuffsicient permission!");
            mails = mails.Trim();
            mails = mails.Replace(" ", string.Empty);

            string[] all = mails.Split(";;");

            foreach(string s in all)
            {
                try
                {
                    AR.ARNews.Mail.Add(s, null);
                }
                catch(Exception ex)
                {

                }
            }
            return Json(mails);
        }
        public IActionResult ADisplayStory(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            try
            {
                AR.ARNews.Clanak.SetStatus(ID, AR.ARNews.ClanakStatus.Published, null);
                return Json("success");
            }
            catch
            {
                return Json("error");
            }
        }
        public IActionResult AHideStory(int ID)
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            try
            {
                AR.ARNews.Clanak.SetStatus(ID, AR.ARNews.ClanakStatus.Draft, null);
                return Json("success");
            }
            catch
            {
                return Json("error");
            }
        }

        [HttpPost]
        public IActionResult AUploadNews(NewsModel Model)
        {
            if (!Networking.isAdmin(Request))
                return Json("Insuffsicient permission!");
            try
            {
                if (string.IsNullOrWhiteSpace(Model.News.Title) || Model.News.Title.Length < 1)
                    return View("Error", new string("You must enter title!"));

                if (Model.Media != null)
                {
                    if (Model.Media.Length > 0)
                    {
                        string[] s = Model.Media.FileName.Split('.');
                        string ext = s[s.Length - 1];

                        switch (ext.ToLower())
                        {
                            case "mp4":
                                Model.News.MediaType = AR.ARNews.News.NewsMediaType.Video;
                                break;

                            case "pdf":
                                Model.News.MediaType = AR.ARNews.News.NewsMediaType.Media;
                                break;

                            default:
                                return View("Error", new String("Not supported format of media!"));
                        }

                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "News") + "\\" + (AR.ARNews.News.MaxID(null) + 1).ToString() + "." + ext;
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            Model.Media.CopyTo(fs);
                        }

                        AR.ARNews.News.Add(Model.News.Title, Model.News.Thumbnail, Model.News.Text, "\\News\\" + (AR.ARNews.News.MaxID(null) + 1).ToString() + "." + ext, Model.News.MediaType, null);
                    }
                }
                else
                {
                    Model.News.MediaType = AR.ARNews.News.NewsMediaType.Null;
                    AR.ARNews.News.Add(Model.News.Title, Model.News.Thumbnail, Model.News.Text, null, Model.News.MediaType, null);
                }

                return Redirect("/Home/News");
            }
            catch(Exception ex)
            {
                return View("Error", new String(ex.ToString()));
            }
        }

        [HttpPost]
        public IActionResult ANewEmployee(AR.ARNews.Employee emp)
        {
            if (!Networking.isAdmin(Request))
                return Json("Insuffsicient permission!");

            try
            {
                AR.ARNews.Employee.Add(emp, null);
            }
            catch(Exception ex)
            {
                return View("Error", new String(ex.ToString()));
            }
            return Redirect("/ControlPanel/Employees");
        }
        public IActionResult AUlogujKorisnika(AR.ARNews.User User)
        {
            try
            {
                if(User.Validate())
                {
                    string newHash;
                    try
                    {
                        ARWebAuthorization.LogUser(User, out newHash);
                    }
                    catch(Exception ex)
                    {
                        return View("Error", "LogUser: " + new String(ex.Message.ToString()));
                    }
                    Response.Cookies.Append("h", newHash);
                    Thread.Sleep(500);
                    return Redirect("/ControlPanel");
                }
                else
                {
                    return View("Error", new String("Passwort oder Benutzername falsch!"));
                }
            }
            catch(Exception ex)
            {
                return View("Error", "User.Validate: " + new String(ex.Message.ToString()));
            }
        }

        [Route("/ControlPanel/EditNews/{id}")]
        public IActionResult EditNews(int id)
        {
            AR.ARNews.News n = AR.ARNews.News.List(null).Where(x => x.NewsID == id).FirstOrDefault();
            return View(n);
        }

        public IActionResult AUpdateNews(AR.ARNews.News n)
        {
            try
            {
                n.Update(null);
            }
            catch(Exception ex)
            {
                return View("Error", new String(ex.ToString()));
            }
            return Redirect("/Home/News");
        }
    }
}