using RealEstater_backend.Data.Models;
using RealEstater_backend.Data.Database;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Repositories
{
    public class LandholdingFeatureRepository : GenericRepository<LandholdingFeatureModel>, ILandholdingFeatureRepository
    {
        public LandholdingFeatureRepository(RealEstaterDbContext realEstaterDbContext) : base(realEstaterDbContext)
        {
        }
    }
}
