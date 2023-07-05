namespace RealEstater_backend.Data.Models
{
    public class LandholdingModel : BaseModel
    {
        public LandholdingModel()
        {
            this.Pictures = new List<LandholdingPictureModel>();
            this.LandholdingFeatures = new List<LandholdingFeatureModel>();
            this.HistoryPrice = new List<PriceHistoryModel>();
        }
        public AddressModel Location { get; set; }
        public LandholdingTypeModel LandholdingType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public ConstructionTypeModel ConstructionType { get; set; }
        public ConstructionStageModel ConstructionStage { get; set; }
        public int YearOfConstruction { get; set; }
        public decimal Area { get; set; }
        public int Floor { get; set; }
        public int NumberOfFloors { get; set; }
        public decimal Courtyard { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public UserModel User { get; set; }
        public List<LandholdingPictureModel> Pictures { get; set; }
        public List<LandholdingFeatureModel> LandholdingFeatures { get; set; }
        public List<PriceHistoryModel> HistoryPrice { get; set; }
        public bool IsActive { get; set; }
    }
}
