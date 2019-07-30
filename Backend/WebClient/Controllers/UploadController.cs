using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClient.ViewModels;

namespace WebClient.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CompareCode()
        {
            //ViewBag.SourceCode = queryCode;
            return View();
        }
    }
}