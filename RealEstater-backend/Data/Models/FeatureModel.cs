namespace RealEstater_backend.Data.Models
{
    public class FeatureModel : BaseModel
    {
        public IEnumerable<LandholdingFeatureModel> LandholdingFeatures { get; set; }
    }
}
