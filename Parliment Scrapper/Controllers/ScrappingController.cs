using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Parliment_Scrapper.Controllers
{
    public class ScrappingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}