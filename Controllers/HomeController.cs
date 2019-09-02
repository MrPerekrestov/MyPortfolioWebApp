using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MyPortfolioWebApp.controllers
{
    public class HomeController : Controller
    {
     
       [Route("")]
       [Route("Home")]
       [Route("Home/About")]
       [HttpGet]
        public IActionResult About()
        {            
            return View();
        }
    }
}
