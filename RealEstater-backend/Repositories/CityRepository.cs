using Microsoft.EntityFrameworkCore;
using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Data.Models;
using RealEstater_backend.Data.Database;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Repositories
{
    public class CityRepository : GenericRepository<CityModel>, ICityRepository
    {
        public CityRepository(RealEstaterDbContext realEstaterDbContext) : base(realEstaterDbContext)
        {
        }

        public List<HistoryPriceDto> GetAveragePricesForRegion(string region, DateTime[] history)
        {
            var allLandholdingsInRegion = this._dbContext.Landholdings
                                                .Include(x => x.Location)
                                                .ThenInclude(y => y.City)
                                                .Include(x => x.HistoryPrice)
                                                .Where(x => x.Location.City.Title == region).ToList();

            var results = new List<HistoryPriceDto>();

            history.ToList().ForEach(x =>
            {
                var totalLandholdings = 0m;
                var totalPrice = 0m;

                allLandholdingsInRegion.ForEach(y =>
                {
                    var relevantHistoryPrices = y.HistoryPrice.Where(p => p.StartDate.Month == x.Month).ToList();
                    if (relevantHistoryPrices.Any())
                    {
                        relevantHistoryPrices.ForEach(c =>
                        {
                            totalLandholdings++;
                            totalPrice += c.Price;
                        });
                    }
                });

                if (totalLandholdings > 0)
                {
                    results.Add(new HistoryPriceDto { Price = totalPrice / totalLandholdings, StartDate = x });
                }
            });

            return results;
        }
    }
}
