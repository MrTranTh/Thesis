using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Thesis.Models.Entity;
using Thesis.Services.SendMailThanhToan;
using X.PagedList;

namespace Thesis.Controllers
{
	public class PostsController : Controller
	{
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment _env;
        private readonly SendMailService _sendMailService;

        public PostsController(AppDbContext dbContext, IWebHostEnvironment env, SendMailService sendMailService)
        {
            db = dbContext;
            _env = env;
            _sendMailService = sendMailService;
        }

        public IActionResult Index(int? page)
		{
			var pageSize = 5;
			if (page == null)
			{
				page = 1;
			}
			IEnumerable<Posts> items = db.Posts.OrderByDescending(x => x.CreatedDate).ToList();
			var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			ViewBag.PageSize = pageSize;
			ViewBag.Page = page;
			items = items.ToPagedList(pageIndex, pageSize);
			return View(items);
		}

		public IActionResult hienThiPost(int id)
		{
            TempData["Id"] = id;
            var item = db.Posts.Where(x => x.Id == id).FirstOrDefault();
			ViewBag.noiDungGanDay = db.Posts.OrderByDescending(x=>x.CreatedDate).Take(3).ToList();
			return View(item);
		}

        [HttpPost]
        public IActionResult NhatXetKhachHang(string hoTen, string email, string diaChi, string loiNhan)
        {
            int id = Convert.ToInt32(TempData["Id"]);
            string infoCustommer = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, "wwwroot", "C:\\Users\\Tran Duc Thang\\Desktop\\Chuyen_de_tot_nghiep\\Thesis\\Thesis\\wwwroot\\TemplateEmail\\send3.html"));
            Random rd = new Random();
            var code = "NX" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            infoCustommer = infoCustommer.Replace("{{TenKhachHang}}", hoTen);
            infoCustommer = infoCustommer.Replace("{{DiaChi}}", diaChi);
            infoCustommer = infoCustommer.Replace("{{Email}}", email);
            infoCustommer = infoCustommer.Replace("{{LoiNhan}}", loiNhan);
            infoCustommer = infoCustommer.Replace("{{Code}}", code);
            MailContent mailContent = new MailContent();
            mailContent.Subject = "Bạn có tin nhắn mới từ khách hàng " + hoTen + " gửi đến từ ngày " + DateTime.Now;
            mailContent.Body = infoCustommer;
            mailContent.To = "tranvantam24003@gmail.com";
            mailContent.Code = code;
            _sendMailService.SendMail(mailContent);
            return RedirectToAction("hienThiPost", new { id = id });
        }
    }
}
