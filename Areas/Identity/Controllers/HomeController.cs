using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Thesis.Models.Identity;

namespace Thesis.Areas.Identity.Controllers
{
    [Authorize]
    [Area("Identity")]
    [Route("/Home/[action]")]
    public class HomeController : Controller
    {
        //private readonly AppDbContext _context;
        //private readonly UserManager<AppUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        //public HomeController(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    _context = context;
        //    _userManager = userManager;
        //    _roleManager = roleManager;
        //}

        public IActionResult Index()
        {
            return View();
        } 


        //public async Task<IActionResult> RootDataAsync()
        //{
        //    var roleNames = typeof(RoleName).GetFields().ToList();
        //    foreach (var item in roleNames)
        //    {
        //        var roleName = (string)item.GetRawConstantValue();
        //        var rFound = await _roleManager.FindByNameAsync(roleName);      
        //        if (rFound == null)
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole(roleName));
        //        }
        //    }

        //    //Admin
        //    var userAdmin = await _userManager.FindByEmailAsync("admin");
        //    if (userAdmin == null) 
        //    {
        //        userAdmin = new AppUser()
        //        {
        //            UserName = "admin",
        //            Email = "tranvantam24003@gmail.com",
        //            EmailConfirmed = true
        //        };

        //        await _userManager.CreateAsync(userAdmin, "admin");
        //        await _userManager.AddToRoleAsync(userAdmin, RoleName.Administrator);
        //    }

        //    return RedirectToAction("/Home/Index");
        //}
    }


}
