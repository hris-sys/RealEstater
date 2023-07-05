using RealEstater_backend.Helpers;
using Microsoft.EntityFrameworkCore;
using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Data.Models;
using RealEstater_backend.Data.Database;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Repositories
{
    public class UserRepository : GenericRepository<UserModel>, IUserRepository
    {
        private readonly ICityRepository cityRepository;

        public UserRepository(RealEstaterDbContext realEstaterDbContext, ICityRepository cityRepository) : base(realEstaterDbContext) 
        {
            this.cityRepository = cityRepository;
        }

        public async Task<UserModel> CreateUser(SignUpUserDto user)
        {
            var allUsers = this.GetAll();

            var existingUser = allUsers.FirstOrDefault(x => x.Email == user.Email);
            if (existingUser != null)
                return null;

            var towns = this.cityRepository.GetAll();

            var newUser = new UserModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = "",
                Password = PasswordHasher.Hash(user.Password),
                PictureURL = PictureHelper.GetDefaultPictureUrl(),
                RegisteredAt = DateTime.UtcNow,
                WebsiteURL = user.WebsiteUrl,
                PhoneNumber = user.PhoneNumber,
                Role = Role.User,
                Landholdings = new List<LandholdingModel>()
            };
            
            this._dbContext.Users.Add(newUser);
            this.SaveChanges();
            return newUser;
        }

        public async Task Delete(int id)
        {
            var user = this._dbContext.Users.Find(id);
            if (user is not null)
            {
                this._dbContext.Users.Remove(user);
                this.SaveChanges();
            }
        }

        public async Task Update(UserModel user)
        {
            this._dbContext.Entry(user).State = EntityState.Modified;
            this.SaveChanges();
        }
    }
}
