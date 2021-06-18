using AppointmentDemo.Data;
using AppointmentDemo.Models;
using AppointmentDemo.helper;
using AppointmentDemo.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
           RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM objvm)
        {
            if (ModelState.IsValid)
            {
                var obj = await _signInManager.PasswordSignInAsync(objvm.Email, objvm.Password, objvm.RememberMe, false);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invlid login Attempt");
            return View(objvm);
        }

        public async Task<IActionResult> Register()
        {
            if (!_roleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
                await _roleManager.CreateAsync(new IdentityRole(Helper.Patiean));
                await _roleManager.CreateAsync(new IdentityRole(Helper.Doctor));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM Robj)
        {
            if (ModelState.IsValid)
            {
                var userobj = new ApplicationUser()
                {
                    UserName = Robj.Email,
                    Email = Robj.Email,
                    Name = Robj.Name
                };

                var result = await _userManager.CreateAsync(userobj, Robj.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userobj, Robj.RoleName);
                    await _signInManager.SignInAsync(userobj, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(Robj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

    }
}
