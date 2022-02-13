using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using project_2_13.Context;
using project_2_13.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace project_2_13.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var users = await context.Users.Select(i => new UserRolesModel
            {
                Id = i.Id,
                UserName = i.UserName
            }).ToListAsync();


            foreach (var user in users)
            {
                var currentInteratedUser = await userManager.FindByIdAsync(user.Id);
                var roleNames = (await userManager.GetRolesAsync(currentInteratedUser)).ToList();
                if (roleNames.Any())
                {
                    user.Roles.AddRange(roleNames);

                }
            }
            //var indexViewModel = new IndexViewModel()
            //{
            //    Users = users
            //};
            return View(users);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
