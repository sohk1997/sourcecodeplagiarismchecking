using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models.DAOs;

namespace WebClient.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("compare/{id}")]
        public ActionResult CompareCode(int id)
        {
            var dao = new ResultDAO();
            var result = dao.GetResult(id).Result;
            Console.WriteLine(id);
            ViewBag.SourceCode = result;
            return View();
        }
    }
}