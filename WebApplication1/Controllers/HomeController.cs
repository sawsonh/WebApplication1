using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.UI.AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Policy = "AdministratorOnly")]
        public IActionResult Index()
        {
            return View();
        }
    }
}