using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace Client_Task_Executor.Pages.Account
{
    /// <summary>
    /// Страница регистрации
    /// </summary>
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> SignInManager;
        private readonly UserManager<User> UserManager;
        /// <summary>
        /// ctor
        /// </summary>
        public RegisterModel(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Данные для регистрации пользователя
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// Логин пользователя
            /// </summary>
            [Required(ErrorMessage = "Поле обязательно!")]
            [Display(Name = "Логин")]
            public string UserName { get; set; }

            /// <summary>
            /// Имя
            /// </summary>
            [Required(ErrorMessage = "Поле обязательно!")]
            [Display(Name = "Имя")]
            public string FirstName { get; set; }

            /// <summary>
            /// Фамилия
            /// </summary>
            [Required(ErrorMessage = "Поле обязательно!")]
            [Display(Name = "Фамилия")]
            public string SurName { get; set; }

            /// <summary>
            /// Пароль
            /// </summary>
            [Required(ErrorMessage = "Поле обязательно!")]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            /// <summary>
            /// Исполнитель
            /// </summary>
            [Display(Name = "Я исполнитель")]
            public bool IAmExecutor { get; set; }
        }

        /// <summary>
        /// Зарегиcтрировать пользователя
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                User user = new()
                {
                    Email = Input.UserName + "@erc.ru",
                    UserName = Input.UserName,
                    FirstName = Input.FirstName,
                    SurName = Input.SurName
                };

                IdentityResult result = await UserManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (Input.IAmExecutor)
                    {
                        await UserManager.AddToRoleAsync(user, "Executor");
                        await SignInManager.PasswordSignInAsync(Input.UserName, Input.Password, true, false);
                        return LocalRedirect(Url.Content("~/"));
                    }
                    else
                    {
                        await UserManager.AddToRoleAsync(user, "Client");
                        await SignInManager.PasswordSignInAsync(Input.UserName, Input.Password, true, false);
                        return LocalRedirect(Url.Content("~/"));
                    }
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, TranslationErrorCode(error.Code));
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
                "DuplicateUserName" => "Такое имя пользователя уже существует",
                "InvalidUserName" => "Имя пользователя не действительно, может содержать только буквы и цифры",
                _ => "Неизвестная ошибка, обратитесь к администратору",
            };
    }
}