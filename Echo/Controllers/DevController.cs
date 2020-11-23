using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Echo.Controllers
{
    public class DevController : Controller
    {
        private IHostingEnvironment _env;

        public DevController(IHostingEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("/ARGallery")]
        public IActionResult Gallery()
        {
            if (!Networking.isAdmin(Request))
                return Redirect("/ControlPanel");

            return View();
        }
        
        public async Task<IActionResult> AChangeCover(IList<IFormFile> files)
        {
            try
            {

                JsonResult r = await AUploadImage(files) as JsonResult;
                string fileN = r.Value as string;

                int y = fileN.IndexOf("src='") + 5;
                int l = fileN.Length - 4 - y;
                fileN = fileN.Substring(y, l);

                Directory.CreateDirectory("arDebug");
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        System.IO.File.WriteAllText("arDebug/mainCoverImg.txt", fileN);
                    }
                    catch (Exception ex)
                    {
                        AR.ARDebug.Log(ex.ToString());
                        Thread.Sleep(500);
                    }
                }

                return Json("1");
            }
            catch(Exception ex)
            {
                AR.ARDebug.Log(ex.ToString());
                return Json("0");
            }
        }

        public async Task<IActionResult> AUploadImage(IList<IFormFile> files)
        {
            string otp = "";
            foreach (IFormFile source in files)
            {
                string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');

                filename = this.EnsureCorrectFilename(filename);

                while (System.IO.File.Exists(this.GetPathAndFilename(filename)))
                {
                    string extension = filename.Substring(filename.Length - 4, 4);
                    filename = filename.Substring(0, filename.Length - 4) + AR.Security.HashPW(Program.rnd.Next(1000).ToString()).Substring(0, 2) + extension;
                }

                using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename)))
                    await source.CopyToAsync(output);

                otp += "<img class='ARGalleryItem' src='/images/" + filename + "' />";
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
    }
}