using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Client_Task_Executor.Pages.Account
{
    /// <summary>
    /// Страница выхода из системы
    /// </summary>
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> SignInManager;

        /// <summary>
        /// ctor
        /// </summary>
        public LogoutModel(SignInManager<User> signInManager)
        {
            SignInManager = signInManager;
        }

        /// <summary>
        /// Выход из системы
        /// </summary>
        public async Task<IActionResult> OnGet()
        {
            await SignInManager.SignOutAsync();
            return LocalRedirect(Url.Content("~/"));
        }
    }
}