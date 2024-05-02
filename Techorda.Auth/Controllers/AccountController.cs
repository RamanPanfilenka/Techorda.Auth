using DA;
using DA.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Techorda.Auth.Models.Account;

namespace Techorda.Auth.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext _context;

        public AccountController(ApplicationContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() 
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid) 
            { 
                var dbUser = await _context.Users
                    .FirstOrDefaultAsync(user => 
                        user.Email == model.Email 
                        && user.Password == model.Password);

                if (dbUser == null) 
                {
                    ModelState.AddModelError("Email", "Unknown user");
                    return View();
                }

                var claims = new List<Claim>() 
                { 
                    new Claim(ClaimsIdentity.DefaultNameClaimType, dbUser.Name),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin"),
                    new Claim("Age", dbUser.Age.ToString()),
                };

                var claimIndentity = new ClaimsIdentity(claims, "ApplicationCookie", 
                    ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimIndentity));

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                var dbUser = await _context.Users
                    .FirstOrDefaultAsync(user => user.Email == model.Email);

                if(dbUser != null) 
                {
                    ModelState.AddModelError("Email", "User with such email already exist");
                    return View();
                }

                var user = new User()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    Age = model.Age
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
