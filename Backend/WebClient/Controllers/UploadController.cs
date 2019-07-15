using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClient.Database;
using WebClient.ViewModels;

namespace WebClient.Controllers
{
    public class UploadController : Controller
    {
        private AppDbContext appDbContext = new AppDbContext();
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CompareCode()
        {
            var queryCode = appDbContext.SourceCodes.Select(x => new SourceCodeModel()
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                StartLine = x.StartLine,
                EndLine = x.EndLine,
                Description = x.Description
            }).Distinct().ToList();
            ViewBag.SourceCode = queryCode;
            return View();
        }
    }
}