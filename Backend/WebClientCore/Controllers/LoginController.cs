using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebClientCore.Models.DAOs;

namespace WebClientCore.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        public IActionResult Post([FromBody]LoginViewModel model)
        {
            System.Console.WriteLine("Start login ");
            LoginDAO dao = new LoginDAO();
            var result = dao.Login(model);
            if(result == null)
            {
                return BadRequest();
            }
            var session = HttpContext.Session;
            var token = "Bearer " + result.Token;
            Response.Cookies.Append("token",token, new Microsoft.AspNetCore.Http.CookieOptions{
                Expires = DateTime.Now.AddMinutes(10),
                HttpOnly = false
            });
            return Redirect("/Submission");
        }
    }
}