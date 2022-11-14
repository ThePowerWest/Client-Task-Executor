using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Client_Task_Executor.Pages
{
    /// <summary>
    /// �������� �������� ������
    /// </summary>
    public class AddTaskModel : PageModel
    {
        private readonly IRepository<ApplicationCore.Entities.Main.Task> TaskRepository;
        private readonly UserManager<User> UserManager;

        /// <summary>
        /// ctor
        /// </summary>
        public AddTaskModel(IRepository<ApplicationCore.Entities.Main.Task> taskRepository,
            UserManager<User> userManager)
        {
            TaskRepository = taskRepository;
            UserManager = userManager;
        }

        /// <summary>
        /// �������� ������
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var currentUser = await UserManager.GetUserAsync(User);

                await TaskRepository.AddAsync(new ApplicationCore.Entities.Main.Task
                {
                    Client = currentUser,
                    Title = Input.Title,
                    Description = Input.Description,
                    DeadLine = Input.DeadLine,
                    Status = "�����"
                });
                return LocalRedirect(Url.Content("~/"));
            }
            return Page();
        }

        /// <summary>
        /// �������� ��� ��������� ������ �� ��������
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// ������ ����� ������ �� ��������
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// ���������
            /// </summary>
            [Required(ErrorMessage = "���� �����������!")]
            [Display(Name = "���������")]
            public string Title { get; set; }

            /// <summary>
            /// �������� ������
            /// </summary>
            [Required(ErrorMessage = "���� �����������!")]
            [Display(Name = "�������� ������")]
            public string Description { get; set; }

            /// <summary>
            /// ���� ����������
            /// </summary>
            [Required(ErrorMessage = "���� �����������!")]
            [DataType(DataType.Date)]
            [Display(Name = "���� ����������")]
            public DateTime DeadLine { get; set; }
        }
    }
}