using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Data.Models;

namespace RealEstater_backend.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<UserModel>
    {
        Task Delete(int id);
        Task<UserModel> CreateUser(SignUpUserDto user);
        Task Update(UserModel user);
    }
}
