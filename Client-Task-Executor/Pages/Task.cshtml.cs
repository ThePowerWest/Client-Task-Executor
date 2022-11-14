using ApplicationCore.Entities.Identity;
using ApplicationCore.Entities.Main;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Client_Task_Executor.Pages
{
    /// <summary>
    /// Страница с полной задачей и комментариями
    /// </summary>
    public class TaskModel : PageModel
    {
        private readonly MainContext Context = new MainContext();
        private readonly IRepository<ApplicationCore.Entities.Main.Task> TaskRepository;
        private readonly UserManager<User> UserManager;

        public ApplicationCore.Entities.Main.Task Task;

        /// <summary>
        /// ctor
        /// </summary>
        public TaskModel(IRepository<ApplicationCore.Entities.Main.Task> taskRepository,
            UserManager<User> userManager)
        {
            TaskRepository = taskRepository;
            UserManager = userManager;
        }

        /// <summary>
        /// Свойство для получения данных со страницы
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Загрузка с главной
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task OnPost(int id)
        {
            Task = await Context.Tasks.Include(t => t.Client)
                .Include(t => t.Executor)
                .Include(t => t.Comments)
                .ThenInclude(c=>c.Commentator)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// Загрузка после комментария или редактирования
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task OnGet(int id)
        {
            Task = await Context.Tasks.Include(t => t.Client)
                .Include(t => t.Executor)
                .Include(t => t.Comments)
                .ThenInclude(c => c.Commentator)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// Оставить комментарий
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAddCommentAsync(int id)
        {
            Task = await Context.Tasks.Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);

            var currentUser = await UserManager.GetUserAsync(User);

            Task.Comments.Add(new Comment
            {   
                Commentator = currentUser,
                Text = Input.Comment,
                DateOfComment = DateTime.Now
            });
            await TaskRepository.UpdateAsync(Task);
            return RedirectToPage("Task", new {id = id});
        }

        /// <summary>
        /// Модель ввода данных со страницы
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// Комментарий
            /// </summary>
            [Display(Name = "Ваш комментарий")]
            public string Comment { get; set; }
        }
    }
}