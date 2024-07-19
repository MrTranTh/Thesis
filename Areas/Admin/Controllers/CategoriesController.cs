using Microsoft.AspNetCore.Mvc;
using Thesis.Models.Entity;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Thesis.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext db;

        public CategoriesController(AppDbContext dbContext)
        {
            db = dbContext;
        }

        //Get
        public IActionResult Index(int ?page) 
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Categories> items = db.Categories.OrderByDescending(x => x.Id);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            items = items.ToPagedList(pageIndex, pageSize);
            return View(items);
        }

        //Insert
        public IActionResult AddCategories()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategories(Categories categories)
        {
            if (ModelState.IsValid)
            {
                categories.CreatedDate = DateTime.Now; 
                categories.ModifiedDate = DateTime.Now;
                db.Categories.Add(categories);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        //Update
        public IActionResult EditCategories(int id)
        {
            var item = db.Categories.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategories(Categories categories)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Attach(categories);
                categories.CreatedDate = DateTime.Now;
                categories.ModifiedDate = DateTime.Now;
                db.Entry(categories).Property(x => x.Title).IsModified = true;
                db.Entry(categories).Property(x => x.Description).IsModified = true;
                db.Entry(categories).Property(x => x.Position).IsModified = true;
                db.Entry(categories).Property(x => x.SeoTitle).IsModified = true;
                db.Entry(categories).Property(x => x.SeoKeywords).IsModified = true;
                db.Entry(categories).Property(x => x.SeoDescription).IsModified = true;
                db.Entry(categories).Property(x => x.Alias).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categories);
        }


        //Delete
        [HttpPost]
        public IActionResult DeleteCategories(int id)
        {
            var item = db.Categories.Find(id);
            if (item != null) {
                db.Categories.Remove(item);
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
                        var obj = db.Categories.Find(Convert.ToInt32(item));
                        db.Categories.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
