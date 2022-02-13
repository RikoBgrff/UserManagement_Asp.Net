using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using project_2_13.Models;
using System.Threading.Tasks;

namespace project_2_13.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly SignInManager<AppUser> signInManager;

        public AuthController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                var result = await userManager.CheckPasswordAsync(user, model.Password);
                if (result)
                {
                    var response = await signInManager.PasswordSignInAsync(user, model.Password, true, true);
                    if (response.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            if (model != null)
            {
                var user = new AppUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                };
                var role = await roleManager.FindByNameAsync("Admin");
                if (role == null)
                {
                    AppRole role2 = new AppRole();
                    role2.Name = "Admin";
                    await roleManager.CreateAsync(role2);
                }
                var result = await userManager.CreateAsync(user, model.Password);
                var response = await userManager.AddToRoleAsync(user, "Admin");
                if (result.Succeeded)
                {
                    Redirect ("Auth/SucsessMessage");
                }
            }
            return View();
        }
    }
}
