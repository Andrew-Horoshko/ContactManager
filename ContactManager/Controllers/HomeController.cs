using ContactManager.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.Controllers
{
    public class HomeController : Controller
    {
        IWebHostEnvironment _appEnvironment;

        private readonly ILogger<HomeController> _logger;

        public HomeController( IWebHostEnvironment appEnvironment ,ILogger<HomeController> logger)
        {
            _appEnvironment = appEnvironment;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/Files/" + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                     uploadedFile.CopyTo(fileStream);
                }
            }
            ContactModel obj = new ContactModel();

            obj.OpenFile(uploadedFile);
            return View(obj);
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
