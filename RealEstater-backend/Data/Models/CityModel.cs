using System.ComponentModel.DataAnnotations;

namespace RealEstater_backend.Data.Models
{
    public class CityModel : BaseModel
    {
        public IEnumerable<AddressModel> Addresses { get; set; }
    }
}
