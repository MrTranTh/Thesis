using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Thesis.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Thesis.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Member")]
    [Area("Admin")]
	public class ProductController : Controller
	{
        private readonly AppDbContext db;
        

        public ProductController(AppDbContext dbContext)
        {
            db = dbContext;    
        }

        //Get
        public IActionResult Index(int? page, int id)
		{
			var pageSize = 5;
			if (page == null)
			{
				page = 1;
			}
			IEnumerable<Product> items = db.Product.OrderByDescending(x => x.CreatedDate);
			var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			ViewBag.PageSize = pageSize;
			ViewBag.Page = page;
			items = items.ToPagedList(pageIndex, pageSize);
			return View(items);
		}

        //Insert
        public IActionResult AddProduct()
        {           
            ViewBag.ProductCategories = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            return View();            
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(Product product, List<string> Images, List<int> rDefault)
        {       
            if (ModelState.IsValid)
            {
                if(Images != null && Images.Count > 0)
                {
                    for(int i = 0; i < Images.Count; i++)
                    {
                        if(i+1 == rDefault[0])
                        {
                            product.Image = Images[i];
                            product.ProductImages.Add(new ProductImages()
                            {
                                ProductId = product.Id,
                                Image = Images[i],
                                isDefault = true
                            }); 
                        }
                        else
                        {
                            product.ProductImages.Add(new ProductImages()
                            {
                                ProductId = product.Id,
                                Image = Images[i],
                                isDefault = false
                            });
                        }
                    }
                }
				
				product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductCategories = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            return View(product);
        }

		//Edit
		public IActionResult EditProduct(int id)
		{
			ViewBag.ProductCategories = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            var items = db.Product.Find(id);
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                product.ModifiedDate = DateTime.Now;
                db.Product.Attach(product);
                db.Entry(product).Property(x => x.Title).IsModified = true;
                db.Entry(product).Property(x => x.ProductCategoriesId).IsModified = true;
                db.Entry(product).Property(x => x.ProductCode).IsModified = true;
                db.Entry(product).Property(x => x.Description).IsModified = true;
                db.Entry(product).Property(x => x.Detail).IsModified = true;
                db.Entry(product).Property(x => x.Quantity).IsModified = true;
                db.Entry(product).Property(x => x.Price).IsModified = true;
                db.Entry(product).Property(x => x.PriceSale).IsModified = true;
                db.Entry(product).Property(x => x.SeoTitle).IsModified = true;
                db.Entry(product).Property(x => x.SeoDescription).IsModified = true;
                db.Entry(product).Property(x => x.SeoKeywords).IsModified = true;
                db.Entry(product).Property(x => x.isActive).IsModified = true;
                db.Entry(product).Property(x => x.isSale).IsModified = true;
                db.Entry(product).Property(x => x.isHot).IsModified = true;
                db.Entry(product).Property(x => x.isFeature).IsModified = true;
                db.Entry(product).Property(x => x.Alias).IsModified = true;
				db.Entry(product).Property(x => x.Image).IsModified = true;
				db.SaveChanges();             
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategories = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            return View(product);
        }

        //Delete
        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            var item = db.Product.Find(id);
            if (item != null)
            {
                db.Product.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        //Tích chọn nhiều ô để xóa
        [HttpPost]
        public IActionResult IsActive(int id)
        {
            var product = db.Product.Find(id);
            if (product != null)
            {
                product.isActive = !product.isActive;
                db.Product.Attach(product);
                db.Entry(product).Property(x => x.Title).IsModified = true;
                db.Entry(product).Property(x => x.ProductCategoriesId).IsModified = true;
                db.Entry(product).Property(x => x.ProductCode).IsModified = true;
                db.Entry(product).Property(x => x.Description).IsModified = true;
                db.Entry(product).Property(x => x.Detail).IsModified = true;
                db.Entry(product).Property(x => x.Quantity).IsModified = true;
                db.Entry(product).Property(x => x.Price).IsModified = true;
                db.Entry(product).Property(x => x.PriceSale).IsModified = true;
                db.Entry(product).Property(x => x.SeoTitle).IsModified = true;
                db.Entry(product).Property(x => x.SeoDescription).IsModified = true;
                db.Entry(product).Property(x => x.SeoKeywords).IsModified = true;
                db.Entry(product).Property(x => x.isActive).IsModified = true;
                db.Entry(product).Property(x => x.isSale).IsModified = true;
                db.Entry(product).Property(x => x.isHot).IsModified = true;
				db.Entry(product).Property(x => x.isHome).IsModified = true;
				db.Entry(product).Property(x => x.isFeature).IsModified = true;
                db.Entry(product).Property(x => x.Alias).IsModified = true;
				db.Entry(product).Property(x => x.Image).IsModified = true;
				db.SaveChanges();
                return Json(new { success = true, isActive = product.isActive });
            }

            return Json(new { success = false });
        }

		[HttpPost]
		public IActionResult IsHome(int id)
		{
			var product = db.Product.Find(id);
			if (product != null)
			{
				product.isHome = !product.isHome;
				db.Product.Attach(product);
				db.Entry(product).Property(x => x.Title).IsModified = true;
				db.Entry(product).Property(x => x.ProductCategoriesId).IsModified = true;
				db.Entry(product).Property(x => x.ProductCode).IsModified = true;
				db.Entry(product).Property(x => x.Description).IsModified = true;
				db.Entry(product).Property(x => x.Detail).IsModified = true;
				db.Entry(product).Property(x => x.Quantity).IsModified = true;
				db.Entry(product).Property(x => x.Price).IsModified = true;
				db.Entry(product).Property(x => x.PriceSale).IsModified = true;
				db.Entry(product).Property(x => x.SeoTitle).IsModified = true;
				db.Entry(product).Property(x => x.SeoDescription).IsModified = true;
				db.Entry(product).Property(x => x.SeoKeywords).IsModified = true;
				db.Entry(product).Property(x => x.isActive).IsModified = true;
				db.Entry(product).Property(x => x.isSale).IsModified = true;
				db.Entry(product).Property(x => x.isHot).IsModified = true;
                db.Entry(product).Property(x => x.isHome).IsModified = true;
				db.Entry(product).Property(x => x.isFeature).IsModified = true;
                db.Entry(product).Property(x => x.Alias).IsModified = true;
                db.SaveChanges();
				return Json(new { success = true, isHome = product.isHome });
			}

			return Json(new { success = false });
		}

		//Xóa nhiều sản phẩm
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
                        var obj = db.Product.Find(Convert.ToInt32(item));
                        db.Product.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        
    }
}
