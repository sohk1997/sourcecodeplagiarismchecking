using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using WebClientCore.Models.DAOs;

namespace WebClient.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        [CustomAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("compare/{id}")]
        [CustomAuthorize]
        public ActionResult CompareCode(int id)
        {
            StringValues token = "";
            Request.Headers.TryGetValue("Authorization",out token);
            var dao = new ResultDAO();
            var result = dao.GetResult(id).Result;
            Console.WriteLine(id);
            ViewBag.SourceCode = result.PeerCheckResult;
            ViewBag.WebCheckResult = result.WebCheckResult;
            return View();
        }
    }
}