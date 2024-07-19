using Microsoft.AspNetCore.Mvc;
using Thesis.Models.Entity;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Thesis.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator,Edit")]
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly AppDbContext db;

        public NewsController(AppDbContext dbContext)
        {
            db = dbContext;
        }

        //Get
        public IActionResult Index(int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<News> items = db.News.OrderByDescending(x => x.Id);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            items = items.ToPagedList(pageIndex, pageSize);
            return View(items);
        }

        //Create
        public IActionResult AddNews()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNews(News news)
        {
            if(ModelState.IsValid)
            {
                news.CategoriesId = db.Categories.FirstOrDefault().Id;
                news.CreatedDate = DateTime.Now;
                news.ModifiedDate = DateTime.Now;
                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        //Update
        public IActionResult EditNews(int id)
        {
            var items = db.News.Find(id);
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditNews(News news)
        {
            if (ModelState.IsValid)
            {
                news.ModifiedDate = DateTime.Now;
                db.News.Attach(news);
                db.Entry(news).Property(x => x.Title).IsModified = true;
                db.Entry(news).Property(x => x.Alias).IsModified = true;
                db.Entry(news).Property(x => x.Image).IsModified = true;
                db.Entry(news).Property(x => x.Description).IsModified = true;
                db.Entry(news).Property(x => x.Detail).IsModified = true;
                db.Entry(news).Property(x => x.SeoTitle).IsModified = true;
                db.Entry(news).Property(x => x.SeoDescription).IsModified = true;
                db.Entry(news).Property(x => x.SeoKeywords).IsModified = true;
                db.Entry(news).Property(x => x.isActive).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        //Delete
        [HttpPost]
        public IActionResult DeleteNews(int id)
        {
            var item = db.News.Find(id);
            if (item != null)
            {
                db.News.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsActive(int id)
        {
            var news = db.News.Find(id);
            if (news != null)
            {
                news.isActive = !news.isActive;
                db.News.Attach(news);
                db.Entry(news).Property(x => x.Title).IsModified = true;
                db.Entry(news).Property(x => x.Alias).IsModified = true;
                db.Entry(news).Property(x => x.Image).IsModified = true;
                db.Entry(news).Property(x => x.Description).IsModified = true;
                db.Entry(news).Property(x => x.Detail).IsModified = true;
                db.Entry(news).Property(x => x.SeoTitle).IsModified = true;
                db.Entry(news).Property(x => x.SeoDescription).IsModified = true;
                db.Entry(news).Property(x => x.SeoKeywords).IsModified = true;
                db.Entry(news).Property(x => x.isActive).IsModified = true;
                db.SaveChanges();
                return Json(new { success = true, isActive = news.isActive });
            }

            return Json(new { success = false });
        }

        //Xóa nhiều tin tức
        [HttpPost]
        public IActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if(items.Any() && items != null)
                {
                    foreach(var item in items)
                    {
                        var obj = db.News.Find(Convert.ToInt32(item));
                        db.News.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
