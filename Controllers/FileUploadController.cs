using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tehotasapaino.Models;
using System.IO;


<<<<<<< HEAD
namespace active_directory_aspnetcore_webapp_openidconnect_v2.Controllers
=======
namespace Tehotasapaino.Controllers
>>>>>>> 4442e294e4ff1fb8c78e9446303f297bb77c82cc
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
