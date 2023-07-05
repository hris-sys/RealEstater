namespace RealEstater_backend.Data.DTOs
{
    public class DisplayLandholdingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public List<string> Pictures { get; set; }
        public List<string> Features { get; set; }
        public List<HistoryPriceDto> HistoryPrices { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LandholdingType { get; set; }
        public string ConstructionMaterial { get; set; }
        public string ConstructionStage { get; set; }
        public int YearOfConstruction { get; set; }
        public decimal Area { get; set; }
        public int Floor { get; set; }
        public int NumberOfFloors { get; set; }
        public decimal Courtyard { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DisplayUserInfoDto User { get; set; }
    }
}
