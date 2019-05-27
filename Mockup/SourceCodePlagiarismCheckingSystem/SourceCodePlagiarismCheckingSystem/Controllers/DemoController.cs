using SourceCodePlagiarismCheckingSystem.Database;
using SourceCodePlagiarismCheckingSystem.Models;
using SourceCodePlagiarismCheckingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SourceCodePlagiarismCheckingSystem.Controllers
{
    public class DemoController : Controller
    {
        private AppDbContext appDbContext = new AppDbContext();
        // GET: Demo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CompareCode()
        {
            var query = appDbContext.Datas.Select(x => new DataRecordModel()
            {
                Id = x.Id,
                Drink = x.Drink,
                Quantity = x.Quantity
            }).Distinct().ToList();
            //Console.WriteLine(query);

            ViewBag.Data = query;
            return View(query);
           
        }
        public ActionResult MyProfile()
        {
            return View();
        }
        public ActionResult MySubmission()
        {
            return View();
        }
    }
}