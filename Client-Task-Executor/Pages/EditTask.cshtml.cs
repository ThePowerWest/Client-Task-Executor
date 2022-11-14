using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Client_Task_Executor.Pages
{
    /// <summary>
    /// Страница редактирования задачи
    /// </summary>
    public class EditTaskModel : PageModel
    {
        private readonly MainContext Context = new MainContext();
        private readonly IRepository<ApplicationCore.Entities.Main.Task> TaskRepository;
        private readonly UserManager<User> UserManager;

        public ApplicationCore.Entities.Main.Task Task;

        /// <summary>
        /// Свойство для получения данных со страницы
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public EditTaskModel(IRepository<ApplicationCore.Entities.Main.Task> taskRepository, 
            UserManager<User> userManager)
        {
            TaskRepository = taskRepository;
            UserManager = userManager;
        }

        /// <summary>
        /// Загрузка страницы
        /// </summary>
        public async Task OnPostAsync(int id)
        {
            Task = await Context.Tasks.Include(t => t.Client)
                .Include(t => t.Executor)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// Изменения от клиента
        /// </summary>
        public async Task<IActionResult> OnPostEditForClientAsync(int id)
        {
            Task = await Context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            Task.DeadLine = Input.DeadLine;

            await TaskRepository.UpdateAsync(Task);
            return RedirectToPage("Task", new { id = id });
        }

        /// <summary>
        /// Изменения от исполнителя
        /// </summary>
        public async Task<IActionResult> OnPostEditForExecutorAsync(int id)
        {
            Task = await Context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            Task.Executor = await UserManager.FindByIdAsync(Input.Executor);
            if (Input.ChangeStatus)
            {
                if (Task.Status == "Новая")
                {
                    Task.Status = "В работе";
                }
                else
                {
                    Task.Status = "Выполнено";
                }
            }

            await TaskRepository.UpdateAsync(Task);
            return RedirectToPage("Task", new { id = id });
        }

        /// <summary>
        /// Модель ввода данных со страницы
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// Срок выполнения
            /// </summary>
            [Display(Name = "Срок выполнения :")]
            [DataType(DataType.Date)]
            public DateTime DeadLine { get; set; }

            /// <summary>
            /// Исполнитель
            /// </summary>
            [Display(Name = "Исполнитель :")]
            public string Executor { get; set; }

            /// <summary>
            /// Изменить статус
            /// </summary>
            [Display(Name = "Изменить статус")]
            public bool ChangeStatus { get; set; }
        }
    }
}