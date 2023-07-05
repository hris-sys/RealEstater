using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Data.Models;

namespace RealEstater_backend.Repositories.Interfaces
{
    public interface ICityRepository : IGenericRepository<CityModel>
    {
        List<HistoryPriceDto> GetAveragePricesForRegion(string region, DateTime[] history);
    }
}
