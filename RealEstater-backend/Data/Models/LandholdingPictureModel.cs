namespace RealEstater_backend.Data.Models
{
    public class LandholdingPictureModel : IdModel
    {
        public string PictureUrl { get; set; }
        public LandholdingModel Landholding { get; set; }
    }
}
