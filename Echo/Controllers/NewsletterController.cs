using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Echo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Echo.Controllers
{
    public class NewsletterController : Controller
    {
        private IHostingEnvironment _he;

        public NewsletterController(IHostingEnvironment he)
        {
            _he = he;
        }

        public IActionResult Subscribe(string mail)
        {
            try
            {
                AR.ARNews.Mail.Add(mail);
            }
            catch(AR.ARException ex)
            {
                return View("Error", new String(ex.ToString()));
            }

            return View("Success", new String("Successfully subscribed to our newsletter!"));
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public IActionResult ASendNewsletter(string Text)
        {
            if (!Networking.isAdmin(Request))
                return Json("Insuffsicient permission!");

            int ttt = 0;
            try
            {
                var smtpClient = new SmtpClient
                {
                    Host = "smtp.office365.com", // set your SMTP server name here
                    Port = 587, // Port 
                    EnableSsl = true,
                    Credentials = new NetworkCredential("no-reply@echosalzburg.at", "VqQ!5*ReCT")
                };

                MailMessage msg = new MailMessage();
                msg.IsBodyHtml = true;
                msg.Subject = "ECHO Salzburg - Newsletter";

                msg.From = new MailAddress("no-reply@echosalzburg.at");


                List<AR.ARNews.Mail> li = AR.ARNews.Mail.List();
                int errors = 0;


                msg.Body = Text;
                msg.Bcc.Add("aleksa@termodom.rs");
                msg.Body += "<br /><a style='display: block; color: gray; text-align: center' href='http://www.echosalzburg.at/ControlPanel/ARemoveMail?id='>Du möchtest künftig keinen Newsletter mehr erhalten? Hier abmelden.</a>";

                smtpClient.Send(msg);
                return Json("Success sent aleksa");


                for (int i = 0; i < li.Count - 1; i++)
                {
                    if (li[i].ID != 1)
                        continue;
                    msg.Body = Text;
                    if (IsValidEmail(li[i].Value))
                    {
                        msg.Bcc.Add(li[i].Value);
                        msg.Body += "<br /><a style='display: block; color: gray; text-align: center' href='http://www.echosalzburg.at/ControlPanel/ARemoveMail?id=" + li[i].ID.ToString() + "&h=" + Security.HashPassword((li[i].ID + 1).ToString()) + "'>Du möchtest künftig keinen Newsletter mehr erhalten? Hier abmelden.</a>";
                    }
                    else
                    {
                        continue;
                    }

                    try
                    {
                        smtpClient.Send(msg);
                    }
                    catch(Exception ex)
                    {
                        errors++;
                    }

                    msg.Bcc.Clear();
                    break;
                }
                return Json("Newsletter successfully sent! [ " + errors  + " ]");
            }
            catch(Exception ex)
            {
                return Json(ex.ToString());
            }
        }
    }
}