using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KendoGridClientSideTest.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace KendoGridClientSideTest.Controllers
{
    public class CategoriesController : Controller
    {

        private ApplicationDbContext _context;
        public CategoriesController()
        {
            _context = new ApplicationDbContext();
        }


        // GET: Categories
        public ActionResult Index()
        {
            var categories = _context.Categories;

            return View(categories);
        }

        public ActionResult CreateCategory()
        {
            var category = new Category();
            return View(category);
        }


        public ActionResult Create(Category category)
        {
            var products = category.Products;
            if (category.Id == 0)
            {
                var cat = _context.Categories.Add(category);
                foreach (var pro in products)
                {
                    pro.CategoryId = cat.Id;
                }
                _context.Products.AddRange(products);
            }
            else
            {
                var cat = _context.Categories.Find(category.Id);
                cat.Name = category.Name;

                foreach (var pro in cat.Products)
                {
                    var newpro = products.SingleOrDefault(p => p.Id == pro.Id);
                    if (newpro == null)
                        _context.Products.Remove(pro);
                    else
                        pro.Name = newpro.Name;
                }

                var newProducts = products.Where(p => p.Id == 0).ToList();
                if (newProducts.Count > 0)
                {
                    foreach (var pro in newProducts)
                    {
                        pro.CategoryId = category.Id;
                    }
                    _context.Products.AddRange(newProducts);
                }

            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {

            Category cat = _context.Categories.SingleOrDefault(c => c.Id == id);
            return View(cat);
        }

        public ActionResult Update(Category category)
        {
            var categoryInDb = _context.Categories.SingleOrDefault(c => c.Id == category.Id);
            categoryInDb.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult EditCategory(int id)
        {
            var category = _context.Categories.SingleOrDefault(c => c.Id == id);
            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            var categoryInDb = _context.Categories.SingleOrDefault(c => c.Id == category.Id);
            if (categoryInDb == null)
                return HttpNotFound();

            if (!ModelState.IsValid)
                return View(category);

            categoryInDb.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Products_Read([DataSourceRequest]DataSourceRequest request, int catId)
        {
            IQueryable<Product> products = _context.Products.Where(p => p.CategoryId == catId);
            DataSourceResult result = products.ToDataSourceResult(request, product => new
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId
            });

            return Json(result);
        }




        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Product> products, int catId)
        {
            var results = new List<Product>();

            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    product.CategoryId = catId;
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    results.Add(product);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Product> products)
        {
            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    var ProductInDb = _context.Products.Find(product.Id);
                    ProductInDb.Name = product.Name;
                    ProductInDb.CategoryId = product.CategoryId;
                    _context.SaveChanges();
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Product> products)
        {
            if (products.Any())
            {
                foreach (var product in products)
                {
                    var prodInDb = _context.Products.Find(product.Id);
                    _context.Products.Remove(prodInDb);
                    _context.SaveChanges();

                }

            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }


        public ActionResult Details(int id)
        {
            var category = _context.Categories.SingleOrDefault(c => c.Id == id);
            if (category == null)
                return HttpNotFound();


            return View(category);
        }

        public ActionResult Delete(int id)
        {
            var category = _context.Categories.SingleOrDefault(c => c.Id == id);
            if (category == null)
                return HttpNotFound();


            var products = _context.Products.Where(p => p.CategoryId == id).ToList();
            if (products.Count > 0)
                _context.Products.RemoveRange(products);


            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }


    }
}