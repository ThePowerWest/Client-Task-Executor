using Ardalis.Specification;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория с CRUD методами
    /// </summary>
    public interface IRepository<T> : IRepositoryBase<T> where T : class
    { }
}