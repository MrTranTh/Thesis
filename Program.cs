using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Thesis.Hubs;
using Thesis.MiddleWare;
using Thesis.Models.Identity;
using Thesis.Services;
using Thesis.Services.PAYPAL;
using Thesis.Services.SendMailIdentity;
using Thesis.Services.VNPAY;

namespace Thesis
{
    public class Program
    {
		public static void Main(string[] args)
        {
            //Biến đọc dữ liệu trong file appsetting.json
            var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .Build();
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container. 
            builder.Services.AddControllersWithViews();


			//Services liên quan đến Session
			builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            //Services liên quan đến gửi mail thanh toán
            builder.Services.AddOptions();
			var mailSettings = config.GetSection("MailSettings");
			builder.Services.Configure<MailSettings>(mailSettings);
			builder.Services.AddTransient<Services.SendMailThanhToan.SendMailService>();

            //Service liên kết tới database
            builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

			//Service tính số lượt truy cập theo ngày, tháng, năm
			builder.Services.AddScoped<AccessLogService>();
            
            //Service cho VNPAY
            builder.Services.AddScoped<IVnPayService, VnPayService>();

            //Service cho PAYPAL
            builder.Services.AddScoped<IPayPalService, PayPalService>();

            //Service gửi mail Identity
            //builder.Services.AddOptions();
            //var mailsetting = config.GetSection("MailSettings");
            //builder.Services.Configure<MailSettings>(mailsetting);
            builder.Services.AddSingleton<IEmailSender, SendMailService>();

            //Service cho Identity
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            //Service thay thế thông báo lỗi mặc định của hệ thống
            builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();

            builder.Services.Configure<IdentityOptions>(options => {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); // Khóa 1 phút
                options.Lockout.MaxFailedAccessAttempts = 10; // Thất bại 10 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất


                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
                options.SignIn.RequireConfirmedAccount = true;

            });

            // Truy cập IdentityOptions
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login/";
                options.LogoutPath = "/logout/";
                options.AccessDeniedPath = "/khongduoctruycap.html";
            });

            //Service cấu hình đăng ký bằng google, facebook
            builder.Services.AddAuthentication()
                    .AddGoogle(options => {
                        var gconfig = config.GetSection("Authentication:Google");
                        options.ClientId = gconfig["ClientId"];
                        options.ClientSecret = gconfig["ClientSecret"];
                        // https://localhost:7047/signin-google
                        options.CallbackPath = "/dang-nhap-tu-google";
                    })
                    .AddFacebook(options => {
                        var fconfig = config.GetSection("Authentication:Facebook");
                        options.AppId = fconfig["AppId"];
                        options.AppSecret = fconfig["AppSecret"];
                        options.CallbackPath = "/dang-nhap-tu-facebook";
                    })
                    // .AddTwitter()
                    // .AddMicrosoftAccount()
                    ;

            builder.Services.AddAuthorization(options => {
                options.AddPolicy("ShowAdminMenu", pb => {
                    pb.RequireRole("Administrator");
                });    
            });

			builder.Services.AddSignalR();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }           
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),"Uploads")
                ),
				//Truy cập vào các tệp tĩnh theo địa chỉ /Uploads/1.jpg
				RequestPath = "/Uploads"
            });
            
            app.UseSession();
            app.ThongKeTruyCapMiddleware();            
            app.UseRouting();

            app.UseAuthentication();//xác thực 
            app.UseAuthorization();//Phân quyền
            
            //Thêm từ file ScaffoldingReadMe.txt để chạy được MVC trong folder Areas
            app.UseEndpoints(endpoints =>
            {
				endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

				//vào trang Home
				endpoints.MapControllerRoute(
				    name: "default",
				    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
                //Trang Giỏ hàng
                endpoints.MapControllerRoute(
                    name: "ShoppingCart",
                    pattern: "{controller=ShoppingCart}/{action=Index}"
                );
                //vào trang Contact
                endpoints.MapControllerRoute(
                    name: "Contact",
                    pattern: "{controller=Contact}/{action=Index}"
                );

                //vào trang Contact
                endpoints.MapControllerRoute(
                    name: "About",
                    pattern: "{controller=About}/{action=Index}"
                );

                //vào trang Shop
                endpoints.MapControllerRoute(
					name: "Product",
					pattern: "{controller=Product}/{action=Index}/{id}"
				);

                //Trang chi tiết sản phẩm
				endpoints.MapControllerRoute(
					name: "ProductDetail",
					pattern: "chi-tiet-san-pham/{alias}-p{id}", // Mẫu tuyến đường mới
					defaults: new { controller = "Product", action = "Detail" }
				);

              
                //vào trang Shop mỗi khi chọn 1 danh mục sản phẩm bên trái
                endpoints.MapControllerRoute(
                    name: "ProductByProductCategories", // Tên của tuyến đường
                    pattern: "danh-muc-san-pham/{alias}-{id}", // Mẫu tuyến đường mới
                    defaults: new { controller = "Product", action = "ProductCategory" }
                );


				//Hiển thị bài viết tương ứng khi bấm vào "Đọc thêm"
				endpoints.MapControllerRoute(
                    name: "PostsDetail",
                    pattern: "bai-viet/{id}",
                    defaults: new { controller = "Posts", action = "hienThiPost" }
                );

				//Hiển thị bài viết tương ứng khi bấm vào "Đọc thêm"
				endpoints.MapControllerRoute(
					name: "NewsDetail",
					pattern: "tin-tuc/{id}",
					defaults: new { controller = "News", action = "hienThiNew" }
				);

                //Tin nhắn live-chat
				endpoints.MapHub<ChatHub>("/chatHub");
			});

            

            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}