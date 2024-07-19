using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Thesis.Models.Entity;
using Microsoft.AspNetCore.Authorization;

namespace Thesis.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator,Edit")]
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly AppDbContext db;

        public PostsController(AppDbContext dbContext)
        {
            db = dbContext;
        }

        //Get
        public IActionResult Index(string search, int? page)
        {
            var pageSize = 6;
            if(page == null)
            {
                page = 1;
            }
            IEnumerable<Posts> items = db.Posts.OrderByDescending(x =>x.Id);
            if (!string.IsNullOrEmpty(search))
            {
                items = items.Where(x => x.Title.Contains(search));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            items = items.ToPagedList(pageIndex, pageSize);
            return View(items);
        }

        //Create
        public IActionResult AddPosts()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPosts(Posts posts)
        {
            if (ModelState.IsValid)
            {
                posts.CategoriesId = db.Categories.FirstOrDefault().Id;
                posts.CreatedDate = DateTime.Now;
                posts.ModifiedDate = DateTime.Now;
                db.Posts.Add(posts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(posts);
        }

        //Update
        public IActionResult EditPosts(int id)
        {
            var items = db.Posts.Find(id);
            return View(items);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPosts(Posts posts)
        {
            if (ModelState.IsValid)
            {
                posts.ModifiedDate = DateTime.Now;
                db.Posts.Attach(posts);
                db.Entry(posts).Property(x => x.Title).IsModified = true;
                db.Entry(posts).Property(x => x.Alias).IsModified = true;
                db.Entry(posts).Property(x => x.Image).IsModified = true;
                db.Entry(posts).Property(x => x.Description).IsModified = true;
                db.Entry(posts).Property(x => x.Detail).IsModified = true;
                db.Entry(posts).Property(x => x.SeoTitle).IsModified = true;
                db.Entry(posts).Property(x => x.SeoDescription).IsModified = true;
                db.Entry(posts).Property(x => x.SeoKeywords).IsModified = true;
                db.Entry(posts).Property(x => x.isActive).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(posts);
        }

        //Delete
        [HttpPost]
        public IActionResult DeletePosts(int id)
        {
            var item = db.Posts.Find(id);
            if (item != null)
            {
                db.Posts.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        //Tích chọn nhiều ô để xóa
        [HttpPost]
        public IActionResult IsActive(int id)
        {
            var posts = db.Posts.Find(id);
            if (posts != null)
            {
                posts.isActive = !posts.isActive;
                db.Posts.Attach(posts);
                db.Entry(posts).Property(x => x.Title).IsModified = true;
                db.Entry(posts).Property(x => x.Alias).IsModified = true;
                db.Entry(posts).Property(x => x.Image).IsModified = true;
                db.Entry(posts).Property(x => x.Description).IsModified = true;
                db.Entry(posts).Property(x => x.Detail).IsModified = true;
                db.Entry(posts).Property(x => x.SeoTitle).IsModified = true;
                db.Entry(posts).Property(x => x.SeoDescription).IsModified = true;
                db.Entry(posts).Property(x => x.SeoKeywords).IsModified = true;
                db.Entry(posts).Property(x => x.isActive).IsModified = true;
                db.SaveChanges();
                return Json(new { success = true, isActive = posts.isActive });
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
                if (items.Any() && items != null)
                {
                    foreach (var item in items)
                    {
                        var obj = db.Posts.Find(Convert.ToInt32(item));
                        db.Posts.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
