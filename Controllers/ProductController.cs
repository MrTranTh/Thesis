using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Thesis.Models.Entity;
using X.PagedList;

namespace Thesis.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext db;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ProductController(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            db = dbContext;
			_httpContextAccessor = httpContextAccessor;
        }

        //Trang Sản phẩm 
        public IActionResult Index(int? page)
		{
			var pageSize = 6;
			if (page == null)
			{
				page = 1;
			}
			IEnumerable<Product> items = db.Product.ToList();
			var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			ViewBag.PageSize = pageSize;
			ViewBag.Page = page;
			items = items.ToPagedList(pageIndex, pageSize);
			return View(items);
		}	

		//Hiện lên các sản phẩm tương ứng vs các Danh mục đã chọn
		public IActionResult ProductCategory(string alias, int id, int? page)
		{
			_httpContextAccessor.HttpContext.Session.SetString("idProductCategory", Convert.ToString(id));
			var pageSize = 6;
			if (page == null)
			{
				page = 1;
			}
			IEnumerable<Product> items = db.Product.ToList();
			if (id >0)
			{
				items = db.Product.Where(x => x.ProductCategoriesId == id).ToList();
			}
			var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			ViewBag.PageSize = pageSize;
			ViewBag.Page = page;
			items = items.ToPagedList(pageIndex, pageSize);
			return View(items);
		}

		//Chi tiết sản phẩm
		public IActionResult Detail(string alias, int id)
		{
			ViewBag.HinhAnhSanPham = db.ProductImages.Where(x => x.ProductId == id).ToList();
			var item = db.Product.Find(id);
			if (item != null)
			{
				db.Product.Attach(item);
				item.ViewCount = item.ViewCount + 1;
				db.Entry(item).Property(x => x.ViewCount).IsModified = true;
				db.SaveChanges();
			}
			return View(item);
		}

		//Lọc gía chung
		[HttpGet]	
		public IActionResult GetProductsByPriceRange(decimal minPrice, decimal maxPrice, int? page)
		{
			_httpContextAccessor.HttpContext.Session.SetString("minPrice", Convert.ToString(minPrice));
			_httpContextAccessor.HttpContext.Session.SetString("maxPrice", Convert.ToString(maxPrice));
			var pageSize = 6;
			if (page == null)
			{
				page = 1;
			}
			IEnumerable<Product> items = db.Product.Where(x => x.Price >= minPrice && x.Price <= maxPrice).ToList(); 
			var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			ViewBag.PageSize = pageSize;
			ViewBag.Page = page;
			items = items.ToPagedList(pageIndex, pageSize);
			return View(items);
		}

		//Lọc gía sau khi lọc sản phẩm
		[HttpGet]
		public IActionResult GetProductsByPriceRangeByProductCategories(int id, decimal minPrice, decimal maxPrice, int? page)
		{
			id = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("idProductCategory"));
			_httpContextAccessor.HttpContext.Session.SetString("minPriceByProductCategory", Convert.ToString(minPrice));
			_httpContextAccessor.HttpContext.Session.SetString("maxPriceByProductCategory", Convert.ToString(maxPrice));
			var pageSize = 6;
			if (page == null)
			{
				page = 1;
			}
			IEnumerable<Product> items = db.Product.Where(x => x.Price >= minPrice && x.Price <= maxPrice && x.ProductCategoriesId == id).ToList();
			var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			ViewBag.PageSize = pageSize;
			ViewBag.Page = page;
			items = items.ToPagedList(pageIndex, pageSize);
			return View(items);
		}

	}
}
