using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Client_Task_Executor.Pages.Account
{
    /// <summary>
    /// Страница авторизации
    /// </summary>
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> SignInManager;

        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Модель ввода данных со страницы
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// Имя пользователя
            /// </summary>
            [Required(ErrorMessage = "Поле обязательно!")]
            [Display(Name = "Имя пользователя")]
            public string UserName { get; set; }

            /// <summary>
            /// Пароль
            /// </summary>
            [Required(ErrorMessage = "Поле обязательно!")]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            /// <summary>
            /// Запомнить пользователя
            /// </summary>
            [Display(Name = "Запомнить меня")]
            public bool RememberMe { get; set; }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public LoginModel(SignInManager<User> signInManager)
        {
            SignInManager = signInManager;
        }

        /// <summary>
        /// Вход в систему
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                SignInResult result = await SignInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, false);
                if (result.Succeeded) return LocalRedirect(Url.Content("~/"));
                if (result.IsLockedOut) return Page();
                else
                {
                    ModelState.AddModelError(string.Empty, TranslationErrorCode(result.ToString()));
                    return Page();
                }
            }
            return Page();
        }

        /// <summary>
        /// Перевод ошибок
        /// </summary>
        /// <param name="code">Код ошибки</param>
        /// <returns>Русифицированная версия полученной ошибки</returns>
        private string TranslationErrorCode(string code) =>
            code switch
            {
                "Failed" => "Не верный логин или пароль",
                _ => "Неизвестная ошибка, обратитесь к администратору",
            };
    }
}