using DerivcoAssessment.Models;

namespace DerivcoAssessment.Services.Interfaces
{
    public interface IBaseService<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(T entity);
    }
}