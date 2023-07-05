using Microsoft.AspNetCore.Mvc;
using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Services.Interfaces;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ICityRepository cityRepository;

        public CityController(IUserService userService, ICityRepository cityRepository)
        {
            this.userService = userService;
            this.cityRepository = cityRepository;
        }

        [HttpGet("getAllCities")]
        public async Task<IActionResult> GetAllCities()
        {
            var result = this.cityRepository.GetAll();
            return Ok(result);
        }


        [HttpPost("getAveragePriceForRegion")]
        public async Task<IActionResult> GetAveragePriceForRegion([FromBody] AvgCityPriceDto input)
        {
            var result = this.cityRepository.GetAveragePricesForRegion(input.City, input.History);
            return Ok(result);
        }
    }
}
