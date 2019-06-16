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

            var queryCode = appDbContext.SourceCodes.Select(x => new SourceCodeRecordModel()
            {
                Id=x.Id,
                Name=x.Name,
                Code=x.Code,
                StartLine=x.StartLine,
                EndLine=x.EndLine,
                Description=x.Description
            }).Distinct().ToList();
            //Console.WriteLine(query);
            
            ViewBag.Data = query;
            ViewBag.SourceCode = queryCode;

            return View();
           
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