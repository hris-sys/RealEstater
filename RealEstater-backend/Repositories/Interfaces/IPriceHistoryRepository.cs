using RealEstater_backend.Data.Models;

namespace RealEstater_backend.Repositories.Interfaces
{
    public interface IPriceHistoryRepository : IGenericRepository<PriceHistoryModel>
    {
        List<PriceHistoryModel> GetPriceHistoryForLandholding(int landholdingId);
    }
}
