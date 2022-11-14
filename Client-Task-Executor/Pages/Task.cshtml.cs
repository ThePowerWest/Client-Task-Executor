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
    /// �������� � ������ ������� � �������������
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
        /// �������� ��� ��������� ������ �� ��������
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// �������� � �������
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
        /// �������� ����� ����������� ��� ��������������
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
        /// �������� �����������
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
        /// ������ ����� ������ �� ��������
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// �����������
            /// </summary>
            [Display(Name = "��� �����������")]
            public string Comment { get; set; }
        }
    }
}