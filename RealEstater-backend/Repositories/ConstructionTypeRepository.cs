using RealEstater_backend.Data.Models;
using RealEstater_backend.Data.Database;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Repositories
{
    public class ConstructionTypeRepository : GenericRepository<ConstructionTypeModel>, IConstructionTypeRepository
    {
        public ConstructionTypeRepository(RealEstaterDbContext realEstaterDbContext) : base(realEstaterDbContext)
        {
        }
    }
}
