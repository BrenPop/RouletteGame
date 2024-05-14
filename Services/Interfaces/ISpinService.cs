using DerivcoAssessment.Models;

namespace DerivcoAssessment.Services.Interfaces
{
    public interface ISpinService : IBaseService<Spin>
    {
        Task<Spin> SpinRouletteWheelAsync();

        Task<Spin?> GetLatestSpinResultAsync();
    }
}