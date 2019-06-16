using SourceCodePlagiarismCheckingSystem.Database;
using SourceCodePlagiarismCheckingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Diagnostics;

namespace SourceCodePlagiarismCheckingSystem.Controllers
{
    public class UserController : Controller
    {
        private AppDbContext appDbContext = new AppDbContext();
        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            var query = appDbContext.Users.Include(x=> x.Country);

            var records = query.Select(x => new UserRecordModel()
            {
                Id = x.Id,
                FullName = string.Concat(x.FirstName, " ", x.LastName),
                DayOfBirth = x.DayOfBirth,
                EmailAddress = x.EmailAddress,
                ProfilePicture=x.ProfilePicture,
                CountryISO3 = x.Country.ISO3,
                isActive=x.isActive

            }).Distinct().ToList();
           
            ViewBag.Users = records;
            return View();
        }
    }
}