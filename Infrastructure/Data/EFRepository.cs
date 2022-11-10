using ApplicationCore.Interfaces;
using Ardalis.Specification.EntityFrameworkCore;

namespace Infrastructure.Data
{
    /// <summary>
    /// EF Репозиторий
    /// </summary>
    public class EFRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class
    {
        protected readonly MainContext Context;

        /// <summary>
        /// ctor
        /// </summary>
        public EFRepository(MainContext context) : base(context)
        {
            Context = context;
        }
    }
}