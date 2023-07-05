using Microsoft.EntityFrameworkCore;
using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Data.Models;
using RealEstater_backend.Data.Database;
using Microsoft.EntityFrameworkCore.Query;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Repositories
{
    public class LandholdingRepository : GenericRepository<LandholdingModel>, ILandholdingRepository
    {
        public LandholdingRepository(RealEstaterDbContext realEstaterDbContext) : base(realEstaterDbContext)
        {
        }

        public override LandholdingModel GetById(int id)
        {
            return this._dbContext.Landholdings
                .Include(x => x.HistoryPrice)
                .Include(x => x.Pictures)
                .Include(x => x.ConstructionStage)
                .Include(x => x.ConstructionType)
                .Include(x => x.LandholdingFeatures).ThenInclude(x => x.Feature)
                .Include(x => x.LandholdingType)
                .Include(x => x.User)
                .Include(x => x.Location).ThenInclude(x => x.City)
                .FirstOrDefault(x => x.Id == id)!; 
        }

        public List<DisplayLandholdingDto> GetLandholdingsByUserId(int userId)
        {
            var dbQuery = this.GetAllLandholdings().Where(x => x.User.Id == userId).ToList();
            var results = new List<DisplayLandholdingDto>();

            foreach (var item in dbQuery)
            {
                results.Add(this.MapResults(item));
            }

            return results;
        }

        public IIncludableQueryable<LandholdingModel, UserModel> GetAllLandholdings()
        {
            return this._dbContext.Landholdings
                .Include(x => x.HistoryPrice)
                .Include(x => x.Pictures)
                .Include(x => x.ConstructionStage)
                .Include(x => x.ConstructionType)
                .Include(x => x.LandholdingFeatures).ThenInclude(x => x.Feature)
                .Include(x => x.LandholdingType)
                .Include(x => x.Location).ThenInclude(x => x.City)
                .Include(x => x.User);
        }

        public DisplayLandholdingDto GetLandholdingById(int id)
        {
            var landholding = this.GetAllLandholdings().Where(x => x.Id == id).FirstOrDefault();

            return this.MapResults(landholding!);
        }
        public DisplayLandholdingDto MapResults(LandholdingModel landholding)
        {
            if (landholding is null)
                return null;

            var mappedPriceHistories = landholding.HistoryPrice.Select(x => new HistoryPriceDto()
            {
                Price = x.Price,
                StartDate = x.StartDate
            }).OrderBy(x => x.StartDate).ToList();

            return new DisplayLandholdingDto
            {
                Id = landholding.Id,
                UserId = landholding.User.Id,
                Address = landholding.Location.Title,
                Area = landholding.Area,
                City = landholding.Location.City.Title,
                YearOfConstruction = landholding.YearOfConstruction,
                Title = landholding.Title,
                ConstructionStage = landholding.ConstructionStage.Title,
                ConstructionMaterial = landholding.ConstructionType.Title,
                LandholdingType = landholding.LandholdingType.Title,
                Courtyard = landholding.Courtyard,
                CreatedAt = landholding.CreatedAt,
                Description = landholding.Description,
                Features = landholding.LandholdingFeatures.Select(x => x.Feature.Title).ToList(),
                Floor = landholding.Floor,
                LastUpdatedAt = landholding.LastUpdatedAt,
                NumberOfFloors = landholding.NumberOfFloors,
                Price = landholding.Price,
                HistoryPrices = mappedPriceHistories,
                Pictures = landholding.Pictures.Select(x => x.PictureUrl).ToList(),
                IsActive = landholding.IsActive,
                User = new DisplayUserInfoDto()
                {
                    Id = landholding.User.Id,
                    Email = landholding.User.Email,
                    LastName = landholding.User.LastName,
                    FirstName = landholding.User.FirstName,
                    PhoneNumber = landholding.User.PhoneNumber,
                    PictureURL = landholding.User.PictureURL,
                    WebsiteURL = landholding.User.WebsiteURL,
                    Landholdings = new List<LandholdingModel>()
                },
            };
        }

        public bool DeleteLandholding(int landholding)
        {
            this._dbContext.Landholdings.Remove(GetById(landholding));
            return _dbContext.SaveChanges() > 0;
        }
    }
}
