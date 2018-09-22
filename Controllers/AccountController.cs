using System.Threading.Tasks;
using Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<MyUser> _userManager;
        private readonly SignInManager<MyUser> _signInManager; //管理登录登出的Manager类。系统自带。
        private readonly RoleManager<MyRole> _roleManager;


        public AccountController(UserManager<MyUser> userManager, SignInManager<MyUser> signInManager, RoleManager<MyRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new MyUser()
            {
                UserName = dto.UserName,
            };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
//                await _userManager.AddToRoleAsync(user, "Member");
                return Content("true");
            }

            return Content("false");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(RegisterDto dto)
        {

            var result = await _signInManager.PasswordSignInAsync(dto.UserName, dto.Password, true, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return Content("login fail");

        }
    }
}