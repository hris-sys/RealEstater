using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using RealEstater_backend.Data.Models;
using RealEstater_backend.Repositories.Interfaces;
using RealEstater_backend.Services.Interfaces;
using RealEstater_backend.Data.DTOs;

namespace RealEstater_backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;
        private byte[] _key;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
            this._key = Encoding.ASCII.GetBytes(this.configuration["Jwt:Key"]);
        }

        public string CreateJwtToken(UserModel user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Email}"),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName}"),
                new Claim("id", $"{user.Id}"),
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(this._key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        public async Task<string> CreateRefreshTokenAsync()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = this.userRepository.GetAll();

            if (tokenInUser.Any(x => x.RefreshToken == refreshToken))
                return await CreateRefreshTokenAsync();

            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.configuration["Jwt:Key"])),
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = false,
                ValidAudience = this.configuration["Jwt:Issuer"],
                ValidIssuer = this.configuration["Jwt:Issuer"],
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principle = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is an invalid token!");

            return principle;
        }

        public async Task<DisplayUserInfoDto> GetUserDisplayInfoByEmail(string email)
        {
            var user = this.userRepository.FindByCondition(x => x.Email == email);
            if (user == null)
                return null;

            return this.GetUserDisplayInfo(user);
        }

        public async Task<DisplayUserInfoDto> GetUserDisplayInfoById(int id)
        {
            var user = this.userRepository.FindByCondition(x => x.Id == id);
            if (user == null)
                return null;

            return this.GetUserDisplayInfo(user);
        }

        private DisplayUserInfoDto GetUserDisplayInfo(UserModel user)
        {
            var returnUser = new DisplayUserInfoDto()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Landholdings = user.Landholdings,
                PhoneNumber = user.PhoneNumber,
                PictureURL = user.PictureURL,
                WebsiteURL = user.WebsiteURL
            };

            return returnUser;
        }

        public async Task<UpdateUserDto> UpdateUserAsync(UserModel existingUser, UpdateUserDto updateUser)
        {
            if (updateUser.FirstName != "")
                existingUser.FirstName = updateUser.FirstName;

            if (updateUser.LastName != "")
                existingUser.LastName = updateUser.LastName;

            if (updateUser.PhoneNumber != "")
                existingUser.PhoneNumber = updateUser.PhoneNumber;

            if (updateUser.WebsiteURL != "")
                existingUser.WebsiteURL = updateUser.WebsiteURL;

            if (updateUser.PictureURL != "")
                existingUser.PictureURL = updateUser.PictureURL;

            if (updateUser.PhoneNumber != "")
                existingUser.PhoneNumber = updateUser.PhoneNumber;

            await this.userRepository.Update(existingUser);
            return updateUser;
        }
    }
}
