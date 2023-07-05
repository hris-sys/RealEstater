using RealEstater_backend.Data.Models;
using RealEstater_backend.Data.Database;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Repositories
{
    public class LandholdingTypeRepository : GenericRepository<LandholdingTypeModel>, ILandholdingTypeRepository
    {
        public LandholdingTypeRepository(RealEstaterDbContext realEstaterDbContext) : base(realEstaterDbContext)
        {
        }
    }
}
