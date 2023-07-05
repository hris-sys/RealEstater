using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Data.Models;
using RealEstater_backend.Repositories.Interfaces;
using RealEstater_backend.Services.Interfaces;

namespace RealEstater_backend.Services
{
    public class LandholdingService : ILandholdingService
    {
        private readonly IUserRepository userRepository;
        private readonly ICityRepository cityRepository;
        private readonly IAdressRepository adressRepository;
        private readonly IFeatureRepository featureRepository;
        private readonly ILandholdingRepository landholdingRepository;
        private readonly IPriceHistoryRepository priceHistoryRepository;
        private readonly ILandholdingTypeRepository landholdingTypeRepository;
        private readonly IConstructionTypeRepository constructionTypeRepository;
        private readonly IConstructionStageRepository constructionStageRepository;
        private readonly ILandholdingFeatureRepository landholdingFeatureRepository;
        private readonly ILandholdingPictureRepository landholdingPictureRepository;

        public LandholdingService(ILandholdingRepository landholdingRepository, ICityRepository cityRepository,
                                  IAdressRepository adressRepository, IPriceHistoryRepository priceHistoryRepository,
                                  IUserRepository userRepository, IConstructionStageRepository constructionStageRepository,
                                  ILandholdingTypeRepository landholdingTypeRepository, IConstructionTypeRepository constructionTypeRepository,
                                  IFeatureRepository featureRepository, ILandholdingFeatureRepository landholdingFeatureRepository,
                                  ILandholdingPictureRepository landholdingPictureRepository)
        {
            this.userRepository = userRepository;
            this.cityRepository = cityRepository;
            this.adressRepository = adressRepository;
            this.featureRepository = featureRepository;
            this.landholdingRepository = landholdingRepository;
            this.priceHistoryRepository = priceHistoryRepository;
            this.landholdingTypeRepository = landholdingTypeRepository;
            this.constructionTypeRepository = constructionTypeRepository;
            this.constructionStageRepository = constructionStageRepository;
            this.landholdingFeatureRepository = landholdingFeatureRepository;
            this.landholdingPictureRepository = landholdingPictureRepository;
        }
        public LandholdingModel CreateLandholding(CreateLandholdingDto createLandholding, string email)
        {
            var userForLandholding = userRepository.FindByCondition(x => x.Email == email);
            if (userForLandholding is null)
                return new LandholdingModel();

            var landholding = new LandholdingModel();

            landholding.CreatedAt = DateTime.Now;
            landholding.LastUpdatedAt = DateTime.Today;
            landholding.User = userForLandholding;
            landholding.Title = createLandholding.Title;
            landholding.Area = createLandholding.Area;
            landholding.Courtyard = createLandholding.Courtyard;
            landholding.Price = createLandholding.Price;
            landholding.Floor = createLandholding.Floor;
            landholding.YearOfConstruction = createLandholding.YearOfConstruction;
            landholding.NumberOfFloors = createLandholding.NumberOfFloors;
            landholding.Description = createLandholding.Description;
            landholding.IsActive = true;

            //create location (city)
            var city = this.cityRepository.FindByCondition(x => x.Title == createLandholding.City);
            var address = this.adressRepository.FindByCondition(x => ((x.Title == createLandholding.Address) && (x.City.Title == createLandholding.City)));

            if (address is not null)
                landholding.Location = address;
            else
            {
                var newAddressModel = new AddressModel();
                newAddressModel.City = city;
                newAddressModel.Title = createLandholding.Address;
                this.adressRepository.Add(newAddressModel);
                this.adressRepository.SaveChanges();
                landholding.Location = this.adressRepository.GetAll().ToList().OrderByDescending(x => x.Id).FirstOrDefault();
            }

            //Landholding type
            landholding.LandholdingType = this.landholdingTypeRepository.FindByCondition(x => x.Title == createLandholding.Type);

            //Construction type
            landholding.ConstructionType = this.constructionTypeRepository.FindByCondition(x => x.Title == createLandholding.ConstructionType);

            //Construction stage
            landholding.ConstructionStage = this.constructionStageRepository.FindByCondition(x => x.Title == createLandholding.ConstructionStage);

            this.landholdingRepository.Add(landholding);
            this.landholdingRepository.SaveChanges();

            //LandholdingFeatures
            createLandholding.Features.ForEach(x =>
            {
                var featureFromDb = this.featureRepository.FindByCondition(y => y.Title == x);
                var featureAndLandholingObject = new LandholdingFeatureModel();

                featureAndLandholingObject.Landholding = landholding;
                featureAndLandholingObject.Feature = featureFromDb;
                this.landholdingFeatureRepository.Add(featureAndLandholingObject);

                landholding.LandholdingFeatures.Add(featureAndLandholingObject);
            });

            //LandholdingPictures
            createLandholding.Pictures.ForEach(x =>
            {
                var pictureForLandholding = new LandholdingPictureModel();

                pictureForLandholding.Landholding = landholding;
                pictureForLandholding.PictureUrl = x;
                this.landholdingPictureRepository.Add(pictureForLandholding);

                landholding.Pictures.Add(pictureForLandholding);
            });

            //create history model
            var priceHistoryModel = new PriceHistoryModel();
            priceHistoryModel.Price = createLandholding.Price;
            priceHistoryModel.StartDate = DateTime.Today;
            priceHistoryModel.Landholding = this.landholdingRepository.GetAll().ToList().OrderByDescending(x => x.Id).FirstOrDefault()!;

            this.priceHistoryRepository.Add(priceHistoryModel);
            this.priceHistoryRepository.SaveChanges();
            landholding.HistoryPrice.Append(priceHistoryModel);

            this.landholdingRepository.SaveChanges();
            return landholding;
        }

        public DisplayLandholdingDto GetLandholdingById(int id)
        {
            return this.landholdingRepository.GetLandholdingById(id);
        }

        public List<DisplayLandholdingDto> GetLandholdingsForUser(string email)
        {
            return this.landholdingRepository.GetLandholdingsByUserId(userRepository.FindByCondition(x => x.Email == email).Id);
        }

        public List<DisplayLandholdingDto> GetLandholdingsForUserById(int id)
        {
            return this.landholdingRepository.GetLandholdingsByUserId(id);
        }

        public LandholdingModel UpdateLandholding(CreateLandholdingDto updateLandholdingObj, string email)
        {
            var userForLandholding = userRepository.FindByCondition(x => x.Email == email);
            if (userForLandholding is null)
                return new LandholdingModel();

            var landholdingToUpdate = this.landholdingRepository.GetById(updateLandholdingObj.Id);
            var city = this.cityRepository.FindByCondition(x => x.Title == updateLandholdingObj.City);
            var address = this.adressRepository.FindByCondition(x => ((x.Title == updateLandholdingObj.Address) && (x.City.Title == updateLandholdingObj.City)));

            if (address is not null)
                landholdingToUpdate.Location = address;
            else
            {
                var newAddressModel = new AddressModel();
                newAddressModel.City = city;
                newAddressModel.Title = updateLandholdingObj.Address;
                this.adressRepository.Add(newAddressModel);
                this.adressRepository.SaveChanges();
                landholdingToUpdate.Location = this.adressRepository.GetAll().ToList().OrderByDescending(x => x.Id).FirstOrDefault();
            }

            landholdingToUpdate.LandholdingType = this.landholdingTypeRepository.FindByCondition(x => x.Title == updateLandholdingObj.Type);
            landholdingToUpdate.ConstructionType = this.constructionTypeRepository.FindByCondition(x => x.Title == updateLandholdingObj.ConstructionType);
            landholdingToUpdate.ConstructionStage = this.constructionStageRepository.FindByCondition(x => x.Title == updateLandholdingObj.ConstructionStage);

            landholdingToUpdate.LandholdingFeatures.ForEach(x =>
            {
                this.landholdingFeatureRepository.Remove(x);
            });

            this.landholdingFeatureRepository.SaveChanges();

            updateLandholdingObj.Features.ForEach(x =>
            {
                var featureFromDb = this.featureRepository.FindByCondition(y => y.Title == x);

                var featureAndLandholingObject = new LandholdingFeatureModel();
                featureAndLandholingObject.Landholding = landholdingToUpdate;
                featureAndLandholingObject.Feature = featureFromDb;

                this.landholdingFeatureRepository.Add(featureAndLandholingObject);
                landholdingToUpdate.LandholdingFeatures.Add(featureAndLandholingObject);
            });

            this.landholdingPictureRepository.GetAll().Where(x => x.Landholding == landholdingToUpdate).ToList().ForEach(x =>
            {
                this.landholdingPictureRepository.Remove(x);
            });

            this.landholdingPictureRepository.SaveChanges();

            updateLandholdingObj.Pictures.ForEach(x =>
            {
                var pictureForLandholding = new LandholdingPictureModel();

                pictureForLandholding.Landholding = landholdingToUpdate;
                pictureForLandholding.PictureUrl = x;
                this.landholdingPictureRepository.Add(pictureForLandholding);

                landholdingToUpdate.Pictures.Add(pictureForLandholding);
            });

            this.landholdingPictureRepository.SaveChanges();

            var priceHistoryModel = new PriceHistoryModel();
            priceHistoryModel.Price = updateLandholdingObj.Price;
            priceHistoryModel.StartDate = DateTime.Today;
            priceHistoryModel.Landholding = landholdingToUpdate;

            this.priceHistoryRepository.Add(priceHistoryModel);
            this.priceHistoryRepository.SaveChanges();

            landholdingToUpdate.HistoryPrice.Append(priceHistoryModel);

            landholdingToUpdate.LastUpdatedAt = DateTime.Today;
            landholdingToUpdate.Title = updateLandholdingObj.Title;
            landholdingToUpdate.Description = updateLandholdingObj.Description;
            landholdingToUpdate.Area = updateLandholdingObj.Area;
            landholdingToUpdate.Courtyard = updateLandholdingObj.Courtyard;
            landholdingToUpdate.Price = updateLandholdingObj.Price;
            landholdingToUpdate.Floor = updateLandholdingObj.Floor;
            landholdingToUpdate.YearOfConstruction = updateLandholdingObj.YearOfConstruction;
            landholdingToUpdate.NumberOfFloors = updateLandholdingObj.NumberOfFloors;
            landholdingToUpdate.IsActive = updateLandholdingObj.IsActive;

            this.landholdingRepository.SaveChanges();
            return new LandholdingModel();
        }

        public List<DisplayLandholdingDto> SearchLandholdings(string query)
        {
            var dbResults = new List<LandholdingModel>();

            if (query == "all")
                dbResults = this.landholdingRepository.GetAllLandholdings().Where(x => x.IsActive).ToList();

            else if (query.Split(';', StringSplitOptions.TrimEntries).Length > 1)
            {
                var filteredQuery = query.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();

                var originalQuerySearch = filteredQuery[0].Split('=')[1] == string.Empty ? null : filteredQuery[0].Split('=')[1];
                var city = filteredQuery[1].Split('=')[1] == string.Empty ? null : filteredQuery[1].Split('=')[1];
                var holdingType = filteredQuery[2].Split('=')[1] == string.Empty ? null : filteredQuery[2].Split('=')[1];
                var stage = filteredQuery[3].Split('=')[1] == string.Empty ? null : filteredQuery[3].Split('=')[1];
                var materialType = filteredQuery[4].Split('=')[1] == string.Empty ? null : filteredQuery[4].Split('=')[1];

                var keywordsList = new List<string>();

                keywordsList.AddRange(this.cityRepository.GetAll().Select(x => x.Title.ToLower()).ToList());
                keywordsList.AddRange(this.constructionStageRepository.GetAll().Select(x => x.Title.ToLower()).ToList());
                keywordsList.AddRange(this.constructionTypeRepository.GetAll().Select(x => x.Title.ToLower()).ToList());
                keywordsList.AddRange(this.landholdingTypeRepository.GetAll().Select(x => x.Title.ToLower()).ToList());

                List<LandholdingModel> allResults;

                if (keywordsList.Contains(originalQuerySearch!.ToLower()))
                    allResults = GetAllFromSearch("all");
                else
                    allResults = GetAllFromSearch(originalQuerySearch!);

                dbResults.AddRange(allResults.Where(x => (city == null || x.Location.City.Title.Contains(city) || x.Location.City.Title.StartsWith(city) || x.Location.City.Title.EndsWith(city)) &&
                                  (stage == null || x.ConstructionStage.Title.Contains(stage) || x.ConstructionStage.Title.StartsWith(stage) || x.ConstructionStage.Title.EndsWith(stage)) &&
                                  (materialType == null || x.ConstructionType.Title.Contains(materialType) || x.ConstructionType.Title.StartsWith(materialType) || x.ConstructionType.Title.EndsWith(materialType)) &&
                                  (holdingType == null || x.LandholdingType.Title.Contains(holdingType) || x.LandholdingType.Title.StartsWith(holdingType) || x.LandholdingType.Title.EndsWith(holdingType))).ToList());
            }
            else
                dbResults = GetAllFromSearch(query);

            var results = new List<DisplayLandholdingDto>();

            foreach (var item in dbResults)
                results.Add(this.landholdingRepository.MapResults(item));

            return results;
        }

        private List<LandholdingModel> GetAllFromSearch(string query)
        {
            if (query == "all")
                return this.landholdingRepository.GetAllLandholdings().Where(x => x.IsActive).ToList();

            return this.landholdingRepository.GetAllLandholdings()
                    .Where(x =>  x.IsActive && (x.Title.Contains(query) || x.Title.StartsWith(query) || x.Title.EndsWith(query) || query == null) ||
                                (x.Location.City.Title.Contains(query) || x.Location.City.Title.StartsWith(query) || x.Location.City.Title.EndsWith(query) || query == null) ||
                                (x.ConstructionStage.Title.Contains(query) || x.ConstructionStage.Title.StartsWith(query) || x.ConstructionStage.Title.EndsWith(query) || query == null) ||
                                (x.ConstructionType.Title.Contains(query) || x.ConstructionType.Title.StartsWith(query) || x.ConstructionType.Title.EndsWith(query) || query == null) ||
                                (x.Location.Title.Contains(query) || x.Location.Title.StartsWith(query) || x.Location.Title.EndsWith(query) || query == null) ||
                                (x.LandholdingType.Title.Contains(query) || x.LandholdingType.Title.StartsWith(query) || x.LandholdingType.Title.EndsWith(query) || query == null)).ToList();
        }

        public List<DisplayLandholdingDto> GetLatestLandholdings()
        {
            var dbResults = this.landholdingRepository.GetAllLandholdings()
                .Where(x => x.Pictures.Any() && x.IsActive)
                .OrderByDescending(x => x.LastUpdatedAt)
                .Take(5)
                .ToList();

            var results = new List<DisplayLandholdingDto>();

            foreach (var item in dbResults)
            {
                results.Add(this.landholdingRepository.MapResults(item));
            }

            return results;
        }

        public List<DisplayLandholdingDto> GetLatestDiscountedLandholdings()
        {
            var dbResults = this.landholdingRepository.GetAllLandholdings()
                .Where(l => l.HistoryPrice.Count >= 2)
                .OrderBy(l => l.LastUpdatedAt)
                .AsEnumerable()
                .Where(l => l.HistoryPrice[l.HistoryPrice.Count - 2].Price > l.HistoryPrice[l.HistoryPrice.Count - 1].Price)
                .Take(8)
                .ToList();

            var results = new List<DisplayLandholdingDto>();

            foreach (var item in dbResults)
            {
                results.Add(this.landholdingRepository.MapResults(item));
            }

            return results;
        }

        public bool DeleteLandholding(int landholdingId)
        {
            var landholdingToDelete = this.landholdingRepository.GetById(landholdingId);

            if (landholdingToDelete is not null)
                return this.landholdingRepository.DeleteLandholding(landholdingId);

            return false;
        }
    }
}
