using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class UserController : Controller
    {
        [CustomAuthorize]
        public IActionResult Index()
        {
            var token = Request.Cookies["token"];
            UserDAO dao = new UserDAO(token);
            ViewBag.Users = dao.GetUsers().Result;
            return View();
        }
    }
}