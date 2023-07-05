using System.ComponentModel.DataAnnotations;

namespace RealEstater_backend.Data.Models
{
    public class AddressModel : BaseModel
    {
        public CityModel City { get; set; }
    }
}
