using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LanchesMac.Controllers
{
    /// <summary>
    /// Controller handling user authentication and authorization.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Displays the login view.
        /// </summary>
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel() { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// Handles user login.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByNameAsync(loginVM.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                if (result.Succeeded)
                {
                    return string.IsNullOrEmpty(loginVM.ReturnUrl) ? RedirectToAction("Index", "Home") : Redirect(loginVM.ReturnUrl);
                }
            }
            ModelState.AddModelError("", "Failed to login");
            return View(loginVM);
        }

        /// <summary>
        /// Displays the user registration view.
        /// </summary>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Handles user registration.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = registerVM.UserName };
                var result = await _userManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("Register", "Error registering the user.");
                }
            }
            return View(registerVM);
        }

        /// <summary>
        /// Logs out the user and redirects to the home page.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.User = null;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Displays the access denied view.
        /// </summary>
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
