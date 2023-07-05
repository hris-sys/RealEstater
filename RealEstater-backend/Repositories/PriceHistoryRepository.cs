using RealEstater_backend.Data.Models;
using RealEstater_backend.Data.Database;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Repositories
{
    public class PriceHistoryRepository : GenericRepository<PriceHistoryModel>, IPriceHistoryRepository
    {
        public PriceHistoryRepository(RealEstaterDbContext realEstaterDbContext) : base(realEstaterDbContext)
        {
        }

        public List<PriceHistoryModel> GetPriceHistoryForLandholding(int landholdingId)
        {
            throw new NotImplementedException();
        }
    }
}
