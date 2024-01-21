using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Exam_CodeAcademy.Helpers;
using MVC_Exam_CodeAcademy.Models;
using MVC_Exam_CodeAcademy.ViewModels;

namespace MVC_Exam_CodeAcademy.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }
            var user = await _userManager.FindByEmailAsync(loginVm.UserNameOrEmail)??await _userManager.FindByNameAsync(loginVm.UserNameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "UserName/Email or Password incorrect");
                return View(loginVm);
            }
            var check = await _userManager.CheckPasswordAsync(user, loginVm.Password);
            if (check == false)
            {
                ModelState.AddModelError("", "UserName/Email or Password incorrect");
                return View(loginVm);
            }
            await _signInManager.SignInAsync(user, true);

            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            if(!ModelState.IsValid)
            {
                return View(registerVm);
            }
            AppUser user = new AppUser()
            {
                Name = registerVm.Name,
                Surname = registerVm.Surname,
                Email = registerVm.Email,
                UserName = registerVm.UserName,
            };
            var checkEmail= await _userManager.FindByEmailAsync(user.Email);
            if (checkEmail!=null)
            {
                ModelState.AddModelError("", "Bu Email Qeydiyyatdan kecib");
                return View();
            }
            var checkUserName = await _userManager.FindByNameAsync(user.UserName);
            if (checkUserName!=null)
            {
                ModelState.AddModelError("", "Bu UserName Istifade olunub");
                return View();
            }

            var result = await _userManager.CreateAsync(user,registerVm.Password);
            if (!result.Succeeded)
            {
                foreach(var item in  result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View();
            }
            //await _userManager.AddToRoleAsync(user,UserRole.Admin.ToString());
            await _userManager.AddToRoleAsync(user,UserRole.Member.ToString());
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> LogOut()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            foreach(UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if(await _roleManager.FindByNameAsync(role.ToString())==null)
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = role.ToString(),
                    });
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
