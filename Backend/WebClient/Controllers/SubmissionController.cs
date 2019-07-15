using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClient.Database;
using WebClient.ViewModels;

namespace WebClient.Controllers
{
    public class SubmissionController : Controller
    {
        private AppDbContext appDbContext = new AppDbContext();
        // GET: Submission
        [HttpGet]
        public ActionResult Index()
        {
           

            var records = appDbContext.Submissions.Select(x => new SubmissionModel()
            {
                Id = x.Id,
                Name=x.Name,
                Status=x.Status

            }).Distinct().ToList();

            ViewBag.Submissions = records;
            return View();
        }
    }
}