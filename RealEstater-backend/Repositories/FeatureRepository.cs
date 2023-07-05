using RealEstater_backend.Data.Models;
using RealEstater_backend.Data.Database;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Repositories
{
    public class FeatureRepository : GenericRepository<FeatureModel>, IFeatureRepository
    {
        public FeatureRepository(RealEstaterDbContext realEstaterDbContext) : base(realEstaterDbContext)
        {
        }
    }
}
