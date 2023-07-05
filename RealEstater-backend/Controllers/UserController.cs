using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using System.Security.Cryptography;
using RealEstater_backend.Data.DTOs;
using Microsoft.IdentityModel.Tokens;
using RealEstater_backend.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using RealEstater_backend.Services.Interfaces;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        private readonly IEmailService emailService;

        public UserController(IUserRepository userRepository, IConfiguration configuration, IUserService userService, IEmailService emailService)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
            this.userService = userService;
            this.emailService = emailService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginUserDto userObj)
        {
            if (userObj == null)
                return BadRequest();

            var user = this.userRepository.FindByCondition(x => x.Email == userObj.Email);

            if (user == null)
                return BadRequest(new { Message = "User with such an email doesn't exist!" });

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
                return BadRequest(new { Message = "Password is incorrect!" });

            user.Token = this.userService.CreateJwtToken(user);
            var newAccessToken = user.Token;

            var newRefreshToken = await this.userService.CreateRefreshTokenAsync();
            user.RefreshToken = newRefreshToken;

            this.userRepository.SaveChanges();

            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] SignUpUserDto userObj)
        {
            var createdUser = this.userRepository.CreateUser(userObj);

            if (createdUser.Result is not null)
            {
                return Ok(new
                {
                    Message = "User registered successfully!"
                });
            }
            else
                return BadRequest(new
                {
                    Message = "User already exists!"
                });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenApiDto tokenApiDto)
        {
            if (tokenApiDto == null)
                return BadRequest("Invalid client request!");

            var accessToken = tokenApiDto.AccessToken;
            var refreshToken = tokenApiDto.RefreshToken;

            var principal = this.userService.GetPrincipalFromExpiredToken(accessToken);
            var email = principal.Identity.Name;

            var user = this.userRepository.FindByCondition(x => x.Email == email);

            if (user is null || user.RefreshToken != refreshToken)
                return BadRequest("Invalid request!");

            var newAccessToken = this.userService.CreateJwtToken(user);
            var newRefreshToken = await this.userService.CreateRefreshTokenAsync();

            user.RefreshToken = newRefreshToken;
            user.Token = newAccessToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(5);
            this.userRepository.SaveChanges();

            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }

        [HttpPost("isAuthenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            AuthDto convertedTokenFromHeader;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                string token = await reader.ReadToEndAsync();
                convertedTokenFromHeader = JsonSerializer.Deserialize<AuthDto>(token);
            }
            if (convertedTokenFromHeader!.AuthToken == "{}")
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
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

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(convertedTokenFromHeader.AuthToken, validationParameters, out validatedToken);
            return Ok(validatedToken);
        }

        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var results = this.userRepository.GetAll();
            return Ok(results);
        }

        [HttpPost("sendEmail/{email}")]
        public async Task<IActionResult> SendResetEmail(string email)
        {
            var user = this.userRepository.FindByCondition(x => x.Email == email);
            if (user == null)
            {
                return NotFound(new
                {
                    Status = 404,
                    Message = "Email doesn't exist!"
                });
            }

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiryTime = DateTime.UtcNow.AddMinutes(15);

            var from = this.configuration["EmailService:From"];
            var emailModel = new ResetPasswordEmailDto(email, "Reset password!", EmailBodyModel.EmailStringBody(email, emailToken));

            this.emailService.SendEmail(emailModel);
            this.userRepository.SaveChanges();

            return Ok(new
            {
                Status = 200,
                Message = "Email sent successfully!"
            });
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
            var user = this.userRepository.FindByCondition(x => x.Email == resetPasswordDto.Email);
            if (user == null)
            {
                return NotFound(new
                {
                    Status = 404,
                    Message = "Email doesn't exist!"
                });
            };
            var tokenCode = user.ResetPasswordToken;
            var expiryTime = user.ResetPasswordExpiryTime;
            if (tokenCode != newToken || expiryTime < DateTime.UtcNow)
            {
                return BadRequest(new
                {
                    Status = 400,
                    Message = "Invalid reset link!"
                });
            }

            user.Password = PasswordHasher.Hash(resetPasswordDto.NewPassword);
            this.userRepository.SaveChanges();

            return Ok(new
            {
                Status = 200,
                Message = "Password reset successfully!"
            });
        }

        [HttpGet("getUserData/{email}")]
        public async Task<IActionResult> GetUserData(string email)
        {
            return Ok(await this.userService.GetUserDisplayInfoByEmail(email));
        }

        [HttpGet("getUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok(await this.userService.GetUserDisplayInfoById(id));
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUserData(UpdateUserDto updateUserDto)
        {
            var userToUpdate = this.userRepository.FindByCondition(x => x.Email == updateUserDto.Email);
            if (userToUpdate is null)
            {
                return BadRequest(new
                {
                    Status = 400,
                    Message = "There is no such user in our system!"
                });
            }
            return Ok(new {
                Message = "User updated successfully!",
                User = await this.userService.UpdateUserAsync(userToUpdate, updateUserDto),
            });
        }
    }
}
