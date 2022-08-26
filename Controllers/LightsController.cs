using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tehotasapaino.Controllers
{
    public class LightsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
