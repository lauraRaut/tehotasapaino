using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tehotasapaino.Models;
using System.IO;
namespace Tehotasapaino.Controllers

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
