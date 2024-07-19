using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Thesis.Models.Entity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Thesis.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator,Member")]
    [Area("Admin")]
	public class ProductCategoriesController : Controller
	{
        private readonly AppDbContext db;

        public ProductCategoriesController(AppDbContext dbContext)
        {
            db = dbContext;
        }

        public IActionResult Index(int? page)
		{
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<ProductCategories> items = db.ProductCategories.OrderByDescending(x => x.Id);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            items = items.ToPagedList(pageIndex, pageSize);
            return View(items);
        }

        //Insert
        public IActionResult AddProductCategories()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProductCategories(ProductCategories model)
        {
            if(ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                db.ProductCategories.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //Update
        public IActionResult EditProductCategories(int id)
        {
            var item = db.ProductCategories.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProductCategories(ProductCategories model)
        {
            if (ModelState.IsValid)
            {
                db.ProductCategories.Attach(model);
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                db.Entry(model).Property(x => x.Title).IsModified = true;
                db.Entry(model).Property(x => x.SeoTitle).IsModified = true;
                db.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
                db.Entry(model).Property(x => x.SeoDescription).IsModified = true;
                db.Entry(model).Property(x => x.isActive).IsModified = true;
                db.Entry(model).Property(x => x.Alias).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        //Delete
        [HttpPost]
        public IActionResult DeleteProductCategories(int id)
        {
            var item = db.ProductCategories.Find(id);
            if (item != null)
            {
                db.ProductCategories.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        //Xóa nhiều 
        [HttpPost]
        public IActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items.Any() && items != null)
                {
                    foreach (var item in items)
                    {
                        var obj = db.ProductCategories.Find(Convert.ToInt32(item));
                        db.ProductCategories.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsActive(int id)
        {
            var productCategories = db.ProductCategories.Find(id);
            if (productCategories != null)
            {
                productCategories.isActive = !productCategories.isActive;
                db.ProductCategories.Attach(productCategories);
                db.Entry(productCategories).Property(x => x.Title).IsModified = true;
                db.Entry(productCategories).Property(x => x.SeoTitle).IsModified = true;
                db.Entry(productCategories).Property(x => x.SeoDescription).IsModified = true;
                db.Entry(productCategories).Property(x => x.SeoKeywords).IsModified = true;
                db.Entry(productCategories).Property(x => x.isActive).IsModified = true;
                db.Entry(productCategories).Property(x => x.Alias).IsModified = true;
                db.SaveChanges();
                return Json(new { success = true, isActive = productCategories.isActive });
            }

            return Json(new { success = false });
        }
    }
}
