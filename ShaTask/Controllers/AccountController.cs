using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ShaTask.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username)
        {
            if (username == null || username.Length < 3)
            {
                return View();

            }
            else
            {
                Claim C1 = new Claim(ClaimTypes.Name, username);
                Claim C2 = new Claim(ClaimTypes.Email, username + "@gmail.com");
                Claim C3 = new Claim(ClaimTypes.Role, "Admin");
                //Claim C4 = new Claim(ClaimTypes.Role, "Instructor");

                ClaimsIdentity CI = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                CI.AddClaim(C1);
                CI.AddClaim(C2);
                CI.AddClaim(C3);
                //CI.AddClaim(C4);

                ClaimsPrincipal cp = new ClaimsPrincipal(CI);

                await HttpContext.SignInAsync(cp);

                return RedirectToAction("index", "Home");

            }


        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}