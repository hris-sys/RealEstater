using Microsoft.EntityFrameworkCore.Query;
using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Data.Models;

namespace RealEstater_backend.Repositories.Interfaces
{
    public interface ILandholdingRepository : IGenericRepository<LandholdingModel>
    {
        List<DisplayLandholdingDto> GetLandholdingsByUserId(int userId);
        DisplayLandholdingDto GetLandholdingById(int id);
        IIncludableQueryable<LandholdingModel, UserModel> GetAllLandholdings();
        DisplayLandholdingDto MapResults(LandholdingModel landholding);
        bool DeleteLandholding(int landholding);
    }
}
