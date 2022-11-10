using ApplicationCore.Entities.Base;
using ApplicationCore.Entities.Identity;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities.Main
{
    /// <summary>
    /// Задача
    /// </summary>
    public class Task : BaseEntity
    {
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public User Executor { get; set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        public User Client { get; set; }

        /// <summary>
        /// Статус выполнения
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Срок выполнения
        /// </summary>
        public DateTime DeadLine { get; set; }

        /// <summary>
        /// Комментарии
        /// </summary>
        public IEnumerable<Comment> Comments { get; set; }
    }
}