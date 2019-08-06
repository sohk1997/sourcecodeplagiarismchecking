using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        [CustomAuthorize]
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}