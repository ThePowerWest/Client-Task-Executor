using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities.Identity
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Фамилия
        /// </summary>
        public string SurName { get; set; } = string.Empty;
    }
}