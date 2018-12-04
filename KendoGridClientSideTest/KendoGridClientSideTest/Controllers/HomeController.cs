using KendoGridClientSideTest.Models;
using System.Web.Mvc;

namespace KendoGridClientSideTest.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }


        public ActionResult Index()
        {
            var categories = _context.Categories;

            return View(categories);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
