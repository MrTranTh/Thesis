using Microsoft.AspNetCore.Mvc;
using Thesis.Services.SendMailThanhToan;

namespace Thesis.Controllers
{
    public class ContactController : Controller
    {
		private readonly IWebHostEnvironment _env;
		private readonly SendMailService _sendMailService;

		public ContactController(IWebHostEnvironment env, SendMailService sendMailService)
		{
			_env = env;
			_sendMailService = sendMailService;
		}

		public IActionResult Index()
        {
            return View();
        }

        [HttpPost] 
		public IActionResult SendMail(string hoTen, string diaChi, string email, string loiNhan) {
			string infoCustommer = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, "wwwroot", "C:\\Users\\Tran Duc Thang\\Desktop\\Chuyen_de_tot_nghiep\\Thesis\\Thesis\\wwwroot\\TemplateEmail\\send3.html"));
            Random rd = new Random();
			var code = "TN" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
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
			return RedirectToAction("Index");
		}
    }
}
