using ApplicationCore.Entities.Identity;
using ApplicationCore.Entities.Main;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_Task_Executor.Pages
{
    /// <summary>
    /// Главная страница
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> UserManager;
        private readonly MainContext Context = new MainContext();
        public IEnumerable<Task> Tasks;
        public IEnumerable<Task> ClientTasks;
        public User CurrentUser;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="userManager"></param>
        public IndexModel(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        /// <summary>
        /// Загрузка страницы
        /// </summary>
        /// <param name="sortBy"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task OnGet(string? sortBy, string? searchString)
        {
            CurrentUser = await UserManager.GetUserAsync(User);

            switch (sortBy)
            {
                case "По статусам":
                    Tasks = Context.Tasks.Include(t => t.Client)
                .Include(t => t.Executor)
                .OrderBy(t=>t.Status);

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        Tasks = Tasks.Where(t => t.Title.ToUpper().Contains(searchString.ToUpper())
                                               || t.Description.ToUpper().Contains(searchString.ToUpper()));
                    }

                    ClientTasks = Tasks.Where(t => t.Client.Id == CurrentUser.Id);
                    break;
                case "По исполнителям":
                    Tasks = Context.Tasks.Include(t => t.Client)
                .Include(t => t.Executor)
                .OrderBy(t=>t.Executor);

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        Tasks = Tasks.Where(t => t.Title.ToUpper().Contains(searchString.ToUpper())
                                               || t.Description.ToUpper().Contains(searchString.ToUpper()));
                    }

                    ClientTasks = Tasks.Where(t => t.Client.Id == CurrentUser.Id);
                    break;
                case "По сроку выполнения":
                    Tasks = Context.Tasks.Include(t => t.Client)
                .Include(t => t.Executor)
                .OrderBy(t => t.DeadLine);

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        Tasks = Tasks.Where(t => t.Title.ToUpper().Contains(searchString.ToUpper())
                                               || t.Description.ToUpper().Contains(searchString.ToUpper()));
                    }

                    ClientTasks = Tasks.Where(t => t.Client.Id == CurrentUser.Id);
                    break;
                default:
                    Tasks = Context.Tasks.Include(t => t.Client)
                .Include(t => t.Executor);

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        Tasks = Tasks.Where(t => t.Title.ToUpper().Contains(searchString.ToUpper())
                                               || t.Description.ToUpper().Contains(searchString.ToUpper()));
                    }

                    ClientTasks = Tasks.Where(t => t.Client.Id == CurrentUser.Id);
                    break;
            }
        }
    }
}