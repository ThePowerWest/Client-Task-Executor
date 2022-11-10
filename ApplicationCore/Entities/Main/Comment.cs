using ApplicationCore.Entities.Base;
using ApplicationCore.Entities.Identity;
using System;

namespace ApplicationCore.Entities.Main
{
    /// <summary>
    /// Комментарий к задаче
    /// </summary>
    public class Comment : BaseEntity
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата комментария
        /// </summary>
        public DateTime DateOfComment { get; set; }
    }
}