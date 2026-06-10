using ClipShare.Core.Entities;
using ClipShare.Utility;
using ClipShare.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClipShare.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model) 
        {
            if (!ModelState.IsValid)
                return View(model);

            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user is null)
                user = await _userManager.FindByEmailAsync(model.UserName);

            if (user is null) 
            {
                model.Erros.Add("Login-Error", "Invalid username or password. Please try again.");
                return View(model);
            }

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded) 
            {
                model.Erros.Add("Login-Error", "Invalid username or password. Please try again.");
                return View(model);
            }

            await HandleSignInUserAsync(user);

            return LocalRedirect(model.ReturnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model) 
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!model.Password.Equals(model.ConfirmPassword)) 
            {
                ModelState.AddModelError("ConfirmPassword", "Confirm password does not match password");
                return View(model);
            }

            if (await CheckEmailExistsAsync(model.Email)) 
            {
                ModelState.AddModelError("Email", $"Email address of {model.Email} is taken, please try using another email address");
                return View(model);
            }

            if (await CheckNameExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name", $"Name address of {model.Name} is taken, please try using another Name");
                return View(model);
            }

            var userToAdd = new AppUser
            {
                Email = model.Email.ToLower(),
                UserName = model.Name.ToLower(),
                Name = model.Name
            };

            var result = await _userManager.CreateAsync(userToAdd, model.Password);
            await _userManager.AddToRoleAsync(userToAdd, SD.UserRole);

            if (!result.Succeeded) 
            {
                foreach (var error in result.Errors) 
                {
                    model.Erros.Add(string.Empty, error.Description);
                }

                return View(model);
            }

            await HandleSignInUserAsync(userToAdd);

            return RedirectToAction("index", "Home");
        }

        public IActionResult AccessDenied()
            => View();

        #region Private Methods

        private async Task<bool> CheckEmailExistsAsync(string email)
            => await _userManager.Users.AnyAsync(u => u.Email.Equals(email.ToLower()));

        private async Task<bool> CheckNameExistsAsync(string name) 
            => await _userManager.Users.AnyAsync(u => u.Name.ToLower().Equals(name.ToLower()));

        private async Task HandleSignInUserAsync(AppUser user) 
        {
            var claimsIndentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            claimsIndentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            claimsIndentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            claimsIndentity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
            claimsIndentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            var roles = await _userManager.GetRolesAsync(user);
            claimsIndentity.AddClaims(
                roles.Select(role => new Claim(ClaimTypes.Role, role))
            );
            var principal = new ClaimsPrincipal(claimsIndentity);

            // using this method in order to assign identityClaims into User.Identity and sign the user in using build in dotnet identity
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
        #endregion

    }
}
