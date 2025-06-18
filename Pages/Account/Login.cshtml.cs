using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CsharpWebAt.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "O nome de usu�rio � obrigat�rio.")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "A senha � obrigat�ria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        // Redireciona o usu�rio de volta para a URL que ele estava tentando acessar
        // antes de ser redirecionado para a p�gina de login.
        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            if (!ModelState.IsValid)
            {
                ErrorMessage = "Preencha todos os campos.";
                return Page();
            }

            if (Username == "admin" && Password == "admin")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Username),
                    new Claim(ClaimTypes.Role, "Administrator") // Atribuir um role, se necess�rio
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Permite que o cookie persista entre sess�es do navegador
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Expira��o do cookie
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redireciona para a URL original ou para a p�gina inicial
                return LocalRedirect(ReturnUrl);
            }
            else
            {
                ErrorMessage = "Nome de usu�rio ou senha inv�lidos.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }
    }
}
