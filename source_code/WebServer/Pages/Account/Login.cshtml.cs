using Armpit.Library.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using WebServer.DatabaseManagement.Repositories;
using WebServer.DataModels.Account;

namespace WebServer.Pages.Account
{
    public class LoginModel : PageModel
    {

        #region Private vars

        private readonly AccountRepository _accountRepository;

        #endregion

        #region Public Props

        [BindProperty]
        public AuthCredentials Credentials { get; set; } = new();

        #endregion

        public LoginModel(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        #region Public Methods

        public IActionResult OnGet()
        {
            var username = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
            if(username == null)
                return Page();
            if (username.ToString().IsNullOrEmpty())
                return Page();
            return GetLoggedInRedirection();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                //authentication
                _accountRepository.ThrowOnIncorrectCreds(Credentials);

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, Credentials.Username)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProps = new AuthenticationProperties();

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProps);

                return GetLoggedInRedirection();
            }
            catch (ArmpitException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }

        private IActionResult GetLoggedInRedirection()
        {
            return Redirect("/");
        }

        #endregion

    }
}
