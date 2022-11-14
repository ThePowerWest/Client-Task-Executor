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
    /// �������� �����������
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
        /// ������ ��� ����������� ������������
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// ����� ������������
            /// </summary>
            [Required(ErrorMessage = "���� �����������!")]
            [Display(Name = "�����")]
            public string UserName { get; set; }

            /// <summary>
            /// ���
            /// </summary>
            [Required(ErrorMessage = "���� �����������!")]
            [Display(Name = "���")]
            public string FirstName { get; set; }

            /// <summary>
            /// �������
            /// </summary>
            [Required(ErrorMessage = "���� �����������!")]
            [Display(Name = "�������")]
            public string SurName { get; set; }

            /// <summary>
            /// ������
            /// </summary>
            [Required(ErrorMessage = "���� �����������!")]
            [DataType(DataType.Password)]
            [Display(Name = "������")]
            public string Password { get; set; }

            /// <summary>
            /// �����������
            /// </summary>
            [Display(Name = "� �����������")]
            public bool IAmExecutor { get; set; }
        }

        /// <summary>
        /// ������c��������� ������������
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
        /// ������� ������
        /// </summary>
        /// <param name="code">��� ������</param>
        /// <returns>���������������� ������ ���������� ������</returns>
        private string TranslationErrorCode(string code) =>
            code switch
            {
                "DuplicateUserName" => "����� ��� ������������ ��� ����������",
                "InvalidUserName" => "��� ������������ �� �������������, ����� ��������� ������ ����� � �����",
                _ => "����������� ������, ���������� � ��������������",
            };
    }
}