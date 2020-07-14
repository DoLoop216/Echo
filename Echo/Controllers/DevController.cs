using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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