using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities.Base
{
    /// <summary>
    /// Базовая сущность
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Самогенерирующийся идентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }
    }
}