using Microsoft.AspNetCore.Mvc;
using Thesis.Models;
using Newtonsoft.Json;
using System.Web.Helpers;
using Thesis.Models.Entity;
using Microsoft.AspNetCore.Hosting;
using Thesis.Services.SendMailThanhToan;

namespace Thesis.Controllers
{
    public class ShoppingCartController : Controller
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IWebHostEnvironment _env;
		private readonly SendMailService _sendMailService;
        private readonly AppDbContext db;


        public ShoppingCartController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env, SendMailService sendMailService, AppDbContext dbContext)
		{
			_httpContextAccessor = httpContextAccessor;
			_env = env;
			_sendMailService = sendMailService;
			db = dbContext;
		}

		/*View liên quan đến thanh toán*/
		public IActionResult CheckOut()
		{
			var cartJson = _httpContextAccessor.HttpContext.Session.GetString("cart");
			ShoppingCart cart;
			if (cartJson != null)
			{
				ViewBag.CheckCart = cartJson;
			}
			return View();
		}

		public IActionResult ChechOutSuccess()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CheckOut(OrderViewModel req)
		{
			var code = new {success = false, code = -1 , msg="Đặt hàng thất bại"};
			if(ModelState.IsValid)
			{
				var cartJson = _httpContextAccessor.HttpContext.Session.GetString("cart");
				ShoppingCart cart;
				if (cartJson != null)
				{
					cart = JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
					Order order = new Order();
					order.CustomerName = req.CustomerName;
					order.Phone = req.Phone;
					order.Address = req.Address;
					order.Email = req.Email;
					order.Message= req.Message;
					order.TypePayment = req.TypePayment;

					cart.Items.ForEach(x => order.OrderDetail.Add(new OrderDetail
					{
						ProductId = x.ProductId,
						Quantity = x.Quantity,
						Price = x.Price,
					}));

					order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
					order.CreatedDate = DateTime.Now;
					order.ModifiedDate = DateTime.Now;
					Random rd = new Random();
					order.Code = "MA" + rd.Next(0,9) + rd.Next(0, 9)+ rd.Next(0, 9)+ rd.Next(0, 9);
					db.Orders.Add(order);
					db.SaveChanges();

					//Gửi mail
					var strSanPham = "";
					var thanhTien = decimal.Zero;
					var tongTien = decimal.Zero;
					foreach(var item in cart.Items)
					{
						strSanPham += "<tr>";
						strSanPham += "<td>" + item.ProductName +"</td>";
						strSanPham += "<td>" + item.Quantity + "</td>";
						strSanPham += "<td>" + string.Format("{0:C}", item.TotalPrice) + "</td>";
						strSanPham += "</tr>";
						thanhTien += item.Price * item.Quantity;
					}
					tongTien = thanhTien;
					//Email gửi khách hàng
					string contentCustommer = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, "wwwroot", "C:\\Users\\Tran Duc Thang\\Desktop\\Chuyen_de_tot_nghiep\\Thesis\\Thesis\\wwwroot\\TemplateEmail\\send2.html"));
					contentCustommer = contentCustommer.Replace("{{MaDonHang}}", order.Code);
					contentCustommer = contentCustommer.Replace("{{SanPham}}", strSanPham);
					contentCustommer = contentCustommer.Replace("{{ThanhTien}}", string.Format("{0:C}", thanhTien));
					contentCustommer = contentCustommer.Replace("{{TongTien}}", string.Format("{0:C}", tongTien));
					contentCustommer = contentCustommer.Replace("{{TenKhachHang}}", order.CustomerName);
					contentCustommer = contentCustommer.Replace("{{SoDienThoai}}", order.Phone);
					contentCustommer = contentCustommer.Replace("{{Email}}", order.Email);
					contentCustommer = contentCustommer.Replace("{{DiaChi}}", order.Address);
					contentCustommer = contentCustommer.Replace("{{NgayDatHang}}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					MailContent mailContent = new MailContent();
					mailContent.Subject = "Duc Thang Boutique";
					mailContent.Body = contentCustommer;
					mailContent.To = order.Email;
					mailContent.Code = order.Code;
					_sendMailService.SendMail(mailContent);

					//Email gửi Admin
					string contenAdmin= System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, "wwwroot", "C:\\Users\\Tran Duc Thang\\Desktop\\Chuyen_de_tot_nghiep\\Thesis\\Thesis\\wwwroot\\TemplateEmail\\send1.html"));
					contenAdmin= contenAdmin.Replace("{{MaDonHang}}", order.Code);
					contenAdmin= contenAdmin.Replace("{{SanPham}}", strSanPham);
					contenAdmin= contenAdmin.Replace("{{ThanhTien}}", string.Format("{0:C}", thanhTien));
					contenAdmin=contenAdmin.Replace("{{TongTien}}", string.Format("{0:C}", tongTien));
					contenAdmin=contenAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
					contenAdmin=contenAdmin.Replace("{{SoDienThoai}}", order.Phone);
					contenAdmin=contenAdmin.Replace("{{Email}}", order.Email);
					contenAdmin=contenAdmin.Replace("{{DiaChi}}", order.Address);
					contenAdmin=contenAdmin.Replace("{{NgayDatHang}}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					MailContent mailContent1 = new MailContent();
					mailContent1.Subject = "Bạn có đơn hàng mới";
					mailContent1.Body = contenAdmin;
					mailContent1.To = "tranvantam24003@gmail.com";
					mailContent1.Code = order.Code;
					_sendMailService.SendMail(mailContent1);
					//Kết thúc gửi mail
					cart.clearCart();
					cartJson = JsonConvert.SerializeObject(cart);

					// Lưu chuỗi JSON vào session
					_httpContextAccessor.HttpContext.Session.SetString("cart", cartJson);
					return Json(new { redirectUrl = Url.Action("ChechOutSuccess", "ShoppingCart") });
				}
			}
			return Json(code);
		}

		//Partial Form thanh toán
		public IActionResult Partial_Form_Check_out()
		{
			return PartialView();
		}
		public OrderViewModel getProductFormCheckOut()
		{
			var cartJson = _httpContextAccessor.HttpContext.Session.GetString("cart");
			ShoppingCart cart;
			if (cartJson != null)
			{
				ViewBag.CheckForm = cartJson;
				return new OrderViewModel();
			}
			else
			{
				return null;
			}
			
		}

		//PartialView tổng tiên phải trả
		public IActionResult Partial_Check_out()
		{
			return PartialView();
		}
		public List<ShoppingCartItems> getProductCheckOut()
		{
			var cartJson = _httpContextAccessor.HttpContext.Session.GetString("cart");
			ShoppingCart cart;
			if (cartJson != null)
			{
				cart = JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
				return cart.Items;
			}
			else
			{
				// Nếu giỏ hàng chưa tồn tại, tạo mới một danh sách mới
				return new List<ShoppingCartItems>();
			}
		}


		/*Kết thúc View liên quan đến thanh toán*/

		/*View liên quan đến đặt hàng*/
		public IActionResult Index()
		{
			return View();
		}

		//PartialView các mặt hàng trong giỏ hàng
		public IActionResult Partial_Item_Cart()
		{
			return PartialView();
		}	
		public List<ShoppingCartItems> getProduct()
		{			
			var cartJson = _httpContextAccessor.HttpContext.Session.GetString("cart");
			ShoppingCart cart;
			if (cartJson != null)
			{
				cart = JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
				return cart.Items;
			}
			else
			{
				// Nếu giỏ hàng chưa tồn tại, tạo mới một danh sách mới
				return new List<ShoppingCartItems>();
			}			
		}
		/*Kết thúc View liên quan đến đặt hàng*/


		//Hiển thị số mặt hàng đã chọn
		[HttpGet]
		public IActionResult ShowCount() 
		{
            var cartJson = _httpContextAccessor.HttpContext.Session.GetString("cart");

            ShoppingCart cart;
            if (cartJson != null)
            {
                cart = JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
                return Json(new { success = true, count = cart.Items.Count });
            }
			return Json(new { success = false, count = 0});
        }
		//Kết thúc Hiển thị số mặt hàng đã chọn

		//Thêm sản phẩm vào Giỏ hàng
		[HttpPost]
		public IActionResult AddToCart(int id, int quantity)
		{
			var code = new { success = false, msg = "", code = -1, count = 0 };
			var checkProduct = db.Product.FirstOrDefault(x => x.Id == id);
			if (checkProduct != null)
			{
				var cartJson = HttpContext.Session.GetString("cart");

				ShoppingCart cart;
				if (cartJson != null)
				{
					// Nếu giỏ hàng đã tồn tại, chuyển đổi chuỗi JSON thành danh sách các mặt hàng
					cart = JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
				}
				else
				{
					// Nếu giỏ hàng chưa tồn tại, tạo mới một danh sách mới
					cart = new ShoppingCart();
				}

				ShoppingCartItems item = new ShoppingCartItems
				{
					ProductId = checkProduct.Id,
					ProductName = checkProduct.Title,
					CategoryId = checkProduct.ProductCategoriesId,
					Quantity = quantity
				};

				if (checkProduct.Image != null)
				{
					item.ProductImage = checkProduct.Image;
				}
				item.Price = checkProduct.Price;
				item.TotalPrice = item.Price * item.Quantity;
				cart.AddToCart(item, quantity);
				cartJson = JsonConvert.SerializeObject(cart);

				// Lưu chuỗi JSON vào session
				_httpContextAccessor.HttpContext.Session.SetString("cart", cartJson);
				code = new { success = true, msg = "Thêm giỏ hàng thành công", code = 1, count = cart.Items.Count };
			}
			return Json(code);
		}
		//Kêt thúc Thêm sản phẩm vào Giỏ hàng


		public IActionResult AddToCartDetail(int id, int quantity)
		{
			var code = new { success = false, msg = "", code = -1, count = 0 };
			var checkProduct = db.Product.FirstOrDefault(x => x.Id == id);
			if (checkProduct != null)
			{
				var cartJson = HttpContext.Session.GetString("cart");

				ShoppingCart cart;
				if (cartJson != null)
				{
					// Nếu giỏ hàng đã tồn tại, chuyển đổi chuỗi JSON thành danh sách các mặt hàng
					cart = JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
				}
				else
				{
					// Nếu giỏ hàng chưa tồn tại, tạo mới một danh sách mới
					cart = new ShoppingCart();
				}

				ShoppingCartItems item = new ShoppingCartItems
				{
					ProductId = checkProduct.Id,
					ProductName = checkProduct.Title,
					CategoryId = checkProduct.ProductCategoriesId,
					Quantity = quantity
				};

				if (checkProduct.Image != null)
				{
					item.ProductImage = checkProduct.Image;
				}
				item.Price = checkProduct.Price;
				item.TotalPrice = item.Price * item.Quantity;
				cart.AddToCart(item, quantity);
				cartJson = JsonConvert.SerializeObject(cart);

				// Lưu chuỗi JSON vào session
				_httpContextAccessor.HttpContext.Session.SetString("cart", cartJson);
				code = new { success = true, msg = "Thêm giỏ hàng thành công", code = 1, count = cart.Items.Count };
			}
			return Json(code);
		}
		//Xóa từng sản phẩm trong giỏ hàng
		[HttpPost]
		public IActionResult Delete(int id)
		{
			var code = new { success = false, msg = "", code = -1, count = 0 };

			var cartJson = _httpContextAccessor.HttpContext.Session.GetString("cart");

			ShoppingCart cart;
			if (cartJson != null)
			{
				// Nếu giỏ hàng đã tồn tại, chuyển đổi chuỗi JSON thành danh sách các mặt hàng
				cart = JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
				var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
				if(checkProduct != null)
				{
					cart.Remove(id);
                    cartJson = JsonConvert.SerializeObject(cart);

                    // Lưu chuỗi JSON vào session
                    _httpContextAccessor.HttpContext.Session.SetString("cart", cartJson);
                    return Json(new { success = true, msg = "Xóa thành công", code = 1, count = cart.Items.Count });
				}
			}
			return Json(code);
		}
		//Kết thúc Xóa từng sản phẩm trong giỏ hàng

		//Cập nhật số lượng sản phẩm tron giỏ hàng
		[HttpPost]
		public IActionResult Update(int id, int quantity)
		{
			var cartJson = _httpContextAccessor.HttpContext.Session.GetString("cart");

			ShoppingCart cart;
			if (cartJson != null)
			{
				// Nếu giỏ hàng đã tồn tại, chuyển đổi chuỗi JSON thành danh sách các mặt hàng
				cart = JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
				cart.UpdateQuantity(id, quantity);
				cartJson = JsonConvert.SerializeObject(cart);

				// Lưu chuỗi JSON vào session
				_httpContextAccessor.HttpContext.Session.SetString("cart", cartJson);
				return Json(new { success = true, count = cart.Items.Count });
			}
			return Json(new { success = false, count = 0 });
		}
		//Kết thúc Cập nhật số lượng sản phẩm tron giỏ hàng

		//Xóa hết giỏ hàng
		[HttpPost]
		public IActionResult DeleteAll()
		{
			var cartJson = _httpContextAccessor.HttpContext.Session.GetString("cart");

			ShoppingCart cart;
			if (cartJson != null)
			{
				// Nếu giỏ hàng đã tồn tại, chuyển đổi chuỗi JSON thành danh sách các mặt hàng
				cart = JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
				cart.clearCart();
                cartJson = JsonConvert.SerializeObject(cart);

                // Lưu chuỗi JSON vào session
                _httpContextAccessor.HttpContext.Session.SetString("cart", cartJson);
                return Json(new { success = true });
			}
			return Json(new { success = false });
		}
		//Kết thúc Xóa hết giỏ hàng


	}
}
