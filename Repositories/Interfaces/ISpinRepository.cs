using DerivcoAssessment.Models;

namespace DerivcoAssessment.Repositories.Interfaces
{
    public interface ISpinRepository : IBaseRepository<Spin>
    {
        Task<Spin?> GetLatestSpinResult();
    }
}
