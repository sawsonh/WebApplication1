using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        //[Authorize]
        //[Authorize(Roles = "Administrator")]
        // AdministratorPolicy AND EmployeeId
        //[Authorize(Policy = "AdministratorPolicy")]
        //[Authorize(Policy = "EmployeeId")]

        public IActionResult Index()
        {
            return View();
        }
    }
}