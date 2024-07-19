using Microsoft.AspNetCore.Mvc;
using Thesis.Models.Entity;

namespace Thesis.Controllers
{
    public class MenuController : Controller
    {
        private readonly AppDbContext db;

        public MenuController(AppDbContext dbContext)
        {
            db = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult MenuTop()
        //{
        //    var items = db.Categories.OrderBy(x=>x.Position).ToList();
        //    return PartialView("MenuTop",items);
        //}

        //Quản lý Menu trên cùng
        public List<Categories> MenuTop()
        {
            var items = db.Categories.OrderBy(x => x.Position).ToList();
            return items;
        }

        //Quản lý các sản phẩm trên Trang chủ
        public List<Product> ProductList()
        {
            var items = db.Product.Where(x=>x.isHome).Take(8).ToList();
            return items;
        }

        //Quản lý Menu các danh mục sản phẩm bên trái trong trang Sản phẩm
        public List<ProductCategories> MenuLeft()
        {
            var items = db.ProductCategories.Where(x=>x.isActive).ToList();
            return items;
        }
    }
}
