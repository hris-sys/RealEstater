namespace RealEstater_backend.Data.DTOs
{
    public class AvgCityPriceDto
    {
        public string City { get; set; }
        public DateTime[] History { get; set; }
    }
}
