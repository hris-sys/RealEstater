using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Data.Models;

namespace RealEstater_backend.Services.Interfaces
{
    public interface ILandholdingService
    {
        LandholdingModel CreateLandholding(CreateLandholdingDto createLandholding, string email);
        List<DisplayLandholdingDto> GetLandholdingsForUser(string email);
        List<DisplayLandholdingDto> GetLandholdingsForUserById(int id);
        DisplayLandholdingDto GetLandholdingById(int id);
        LandholdingModel UpdateLandholding(CreateLandholdingDto updateLandholdingObj, string email);
        List<DisplayLandholdingDto> SearchLandholdings(string query);
        List<DisplayLandholdingDto> GetLatestLandholdings();
        List<DisplayLandholdingDto> GetLatestDiscountedLandholdings();
        bool DeleteLandholding(int landholdingId);
    }
}
