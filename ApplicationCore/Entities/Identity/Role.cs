using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities.Identity
{
    /// <summary>
    /// Роль пользователя
    /// </summary>
    public class Role : IdentityRole
    {
        /// <summary>
        /// Описание роли
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}