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
    /// �������� �������������� ������
    /// </summary>
    public class EditTaskModel : PageModel
    {
        private readonly MainContext Context = new MainContext();
        private readonly IRepository<ApplicationCore.Entities.Main.Task> TaskRepository;
        private readonly UserManager<User> UserManager;

        public ApplicationCore.Entities.Main.Task Task;

        /// <summary>
        /// �������� ��� ��������� ������ �� ��������
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
        /// �������� ��������
        /// </summary>
        public async Task OnPostAsync(int id)
        {
            Task = await Context.Tasks.Include(t => t.Client)
                .Include(t => t.Executor)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// ��������� �� �������
        /// </summary>
        public async Task<IActionResult> OnPostEditForClientAsync(int id)
        {
            Task = await Context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            Task.DeadLine = Input.DeadLine;

            await TaskRepository.UpdateAsync(Task);
            return RedirectToPage("Task", new { id = id });
        }

        /// <summary>
        /// ��������� �� �����������
        /// </summary>
        public async Task<IActionResult> OnPostEditForExecutorAsync(int id)
        {
            Task = await Context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            Task.Executor = await UserManager.FindByIdAsync(Input.Executor);
            if (Input.ChangeStatus)
            {
                if (Task.Status == "�����")
                {
                    Task.Status = "� ������";
                }
                else
                {
                    Task.Status = "���������";
                }
            }

            await TaskRepository.UpdateAsync(Task);
            return RedirectToPage("Task", new { id = id });
        }

        /// <summary>
        /// ������ ����� ������ �� ��������
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// ���� ����������
            /// </summary>
            [Display(Name = "���� ���������� :")]
            [DataType(DataType.Date)]
            public DateTime DeadLine { get; set; }

            /// <summary>
            /// �����������
            /// </summary>
            [Display(Name = "����������� :")]
            public string Executor { get; set; }

            /// <summary>
            /// �������� ������
            /// </summary>
            [Display(Name = "�������� ������")]
            public bool ChangeStatus { get; set; }
        }
    }
}