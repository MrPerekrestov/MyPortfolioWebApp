using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace MyPortfolioWebApp.controllers
{
    public class HomeController : Controller
    {
       // [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
       [Route("")]
       [Route("Home")]
       [Route("Home/About")]
        public IActionResult About()
        {            
            return View();
        }
    }
}
