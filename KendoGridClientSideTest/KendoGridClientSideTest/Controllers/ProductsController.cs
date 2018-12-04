using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KendoGridClientSideTest.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace KendoGridClientSideTest.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products_Read([DataSourceRequest]DataSourceRequest request, int? catId)
        {
            IQueryable<Product> products;
            DataSourceResult result;
            if (catId != null)
            {
                products = db.Products.Where(p => p.CategoryId == catId);
                result = products.ToDataSourceResult(request, product => new
                {
                    Id = product.Id,
                    Name = product.Name,
                    CategoryId = catId
                });
            }
            else
            {
                products = db.Products;
                result = products.ToDataSourceResult(request, product => new
                {
                    Id = product.Id,
                    Name = product.Name,
                });
            }

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Update([DataSourceRequest]DataSourceRequest request, Product product)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                };

                db.Products.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Destroy([DataSourceRequest]DataSourceRequest request, Product product)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                };

                db.Products.Attach(entity);
                db.Products.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
