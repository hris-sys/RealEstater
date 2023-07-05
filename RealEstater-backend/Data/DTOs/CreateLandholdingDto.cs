namespace RealEstater_backend.Data.DTOs
{
    public class CreateLandholdingDto
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string ConstructionType { get; set; }
        public string ConstructionStage { get; set; }
        public int YearOfConstruction { get; set; }
        public decimal Area { get; set; }
        public int Floor { get; set; }
        public int NumberOfFloors { get; set; }
        public decimal Courtyard { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public List<string> Features { get; set; }
        public List<string> Pictures { get; set; }
    }
}
