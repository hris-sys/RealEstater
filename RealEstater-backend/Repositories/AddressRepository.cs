using RealEstater_backend.Data.Database;
using RealEstater_backend.Data.Models;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Repositories
{
    public class AddressRepository : GenericRepository<AddressModel>, IAdressRepository
    {
        public AddressRepository(RealEstaterDbContext realEstaterDbContext) : base(realEstaterDbContext)
        {

        }
    }
}
