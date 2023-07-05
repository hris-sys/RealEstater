using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Data.Models;
using System.Security.Claims;

namespace RealEstater_backend.Services.Interfaces
{
    public interface IUserService
    {
        string CreateJwtToken(UserModel user);
        Task<string> CreateRefreshTokenAsync();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<DisplayUserInfoDto> GetUserDisplayInfoByEmail(string email);
        Task<DisplayUserInfoDto> GetUserDisplayInfoById(int id);
        Task<UpdateUserDto> UpdateUserAsync(UserModel existingUser, UpdateUserDto updateUser); 
    }
}
