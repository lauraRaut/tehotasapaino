using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace active_directory_aspnetcore_webapp_openidconnect_v2.Controllers
{
    public class LightsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
