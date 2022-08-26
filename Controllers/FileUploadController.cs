using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using active_directory_aspnetcore_webapp_openidconnect_v2.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace active_directory_aspnetcore_webapp_openidconnect_v2.Controllers
{
    public class FileUploadController : Controller
    {
       public IActionResult Index()
        {
            SingleFileModel model = new SingleFileModel();
            return View(model);
        }

        public IActionResult FileUploadView()
        {
            return View();
        }

      
    }
}
