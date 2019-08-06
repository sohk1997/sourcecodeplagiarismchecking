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
        [CustomAuthorize]
        public ActionResult Index()
        {
            var token = Request.Cookies["token"];
            System.Console.WriteLine("token" + token);
            SubmissionDAO dao = new SubmissionDAO(token);
            var records = dao.GetAllSubmission().Result;
            ViewBag.Submissions = records;
            return View();
        }
    }
}