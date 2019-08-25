using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class SourceCodeController : Controller
    {
        // GET: SourceCode
        [HttpGet]
        [CustomAuthorize]
        public IActionResult Index()
        {
            //var token = Request.Cookies["token"];
            //System.Console.WriteLine("token" + token);
            //SourceCodeDAO dao = new SourceCodeDAO(token);
            //var records = dao.GetAllSubmission().Result;
            //ViewBag.SourceCodes = records;
            return View();
        }
    }
}