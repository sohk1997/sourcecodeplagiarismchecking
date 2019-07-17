using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using WebClientCore.Models.DAOs;

namespace WebClient.Controllers
{
    public class SubmissionController : Controller
    {
        // GET: Submission
        [HttpGet]
        public ActionResult Index()
        {
            SubmissionDAO dao = new SubmissionDAO();
            var records = dao.GetAllSubmission().Result;
            ViewBag.Submissions = records;
            return View();
        }
    }
}