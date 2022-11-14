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
    /// Страница создания задачи
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
        /// Создание задачи
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
                    Status = "Новая"
                });
                return LocalRedirect(Url.Content("~/"));
            }
            return Page();
        }

        /// <summary>
        /// Свойство для получения данных со страницы
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Модель ввода данных со страницы
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// Заголовок
            /// </summary>
            [Required(ErrorMessage = "Поле обязательно!")]
            [Display(Name = "Заголовок")]
            public string Title { get; set; }

            /// <summary>
            /// Описание задачи
            /// </summary>
            [Required(ErrorMessage = "Поле обязательно!")]
            [Display(Name = "Описание задачи")]
            public string Description { get; set; }

            /// <summary>
            /// Срок выполнения
            /// </summary>
            [Required(ErrorMessage = "Поле обязательно!")]
            [DataType(DataType.Date)]
            [Display(Name = "Срок выполнения")]
            public DateTime DeadLine { get; set; }
        }
    }
}