namespace RealEstater_backend.Data.Models
{
    public class PriceHistoryModel : IdModel
    {
        public DateTime StartDate { get; set; }
        public decimal Price { get; set; }
        public LandholdingModel Landholding { get; set; }
    }
}
