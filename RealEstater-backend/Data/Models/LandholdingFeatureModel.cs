namespace RealEstater_backend.Data.Models
{
    public class LandholdingFeatureModel : IdModel
    {
        public int LandholdingId { get; set; }
        public LandholdingModel Landholding { get; set; }
        public int FeatureId { get; set; }
        public FeatureModel Feature { get; set; }
    }
}
