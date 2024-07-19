using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using Thesis.Models.Entity;

namespace Thesis.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator,Member")]
    [Area("Admin")]
	public class ProductImagesController : Controller
	{
        private readonly AppDbContext db;

        public ProductImagesController(AppDbContext dbContext)
        {
            db = dbContext;
        }

        public IActionResult Index(int id)
		{
			ViewBag.ProductId = id;
			var items = db.ProductImages.Where(x => x.ProductId == id).ToList();
			return View(items);
		}

		//Delete
		[HttpPost]
		public IActionResult DeleteProductImages(int id)
		{
			var item = db.ProductImages.Find(id);
			if (item != null)
			{
				db.ProductImages.Remove(item);
				db.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}

		//Add
		[HttpPost]
		public IActionResult AddProductImages(int productid, string url)
		{
			db.ProductImages.Add(new ProductImages
			{
				ProductId = productid,
				Image = url,
				isDefault = false
			});
			
			db.SaveChanges();
			return Json(new { success = true });
		}

		[HttpPost]
		public IActionResult chonAnhDaiDien(int id)
		{		
			var item = db.ProductImages.Find(id);
			var products = db.ProductImages.Where(x =>x.ProductId == item.ProductId).ToList();
			foreach(var x in products)
			{
				x.isDefault = false;
			}
			if(item != null)
			{
				item.isDefault = true;
			}
			var product = (from itemProducts in db.Product where itemProducts.Id == item.ProductId select itemProducts).FirstOrDefault();
			var productEntry = db.Entry(product);
			productEntry.Collection(x => x.ProductImages).Load();
			var productAvatar = (from c in product.ProductImages where c.ProductId == item.ProductId && c.isDefault == true select c).FirstOrDefault();
			product.Image = productAvatar.Image;
			
			db.Product.Update(product);
			db.SaveChanges();

			//if (item != null)
			//{
			//	var entity = db.Product.Where(x => x.Id == item.ProductId).FirstOrDefault();
			//	entity.Image = item.Image;

			//	db.Product.Update(entity);
			//	db.SaveChanges();
			//	return Json(new { success = true });
			//}
			return Json(new { success = true });


		}
	}
}
