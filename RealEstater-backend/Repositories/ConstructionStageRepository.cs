using RealEstater_backend.Data.Models;
using RealEstater_backend.Data.Database;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Repositories
{
    public class ConstructionStageRepository : GenericRepository<ConstructionStageModel>, IConstructionStageRepository
    {
        public ConstructionStageRepository(RealEstaterDbContext realEstaterDbContext) : base(realEstaterDbContext)
        {
        }
    }
}
