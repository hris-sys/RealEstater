using System.Text;
using RealEstater_backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstater_backend.Repositories;
using RealEstater_backend.Data.Database;
using RealEstater_backend.Services.Interfaces;
using RealEstater_backend.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ICityRepository, CityRepository>();
builder.Services.AddTransient<IReplyRepository, ReplyRepository>();
builder.Services.AddTransient<IStatusRepository, StatusRepository>();
builder.Services.AddTransient<IAdressRepository, AddressRepository>();
builder.Services.AddTransient<IFeatureRepository, FeatureRepository>();
builder.Services.AddTransient<ILandholdingService, LandholdingService>();
builder.Services.AddTransient<IEmailService, ResetPasswordEmailService>();
builder.Services.AddTransient<IConversationService, ConversationService>();
builder.Services.AddTransient<ILandholdingRepository, LandholdingRepository>();
builder.Services.AddTransient<IPriceHistoryRepository, PriceHistoryRepository>();
builder.Services.AddTransient<IConversationRepository, ConversationRepository>();
builder.Services.AddTransient<ILandholdingTypeRepository, LandholdingTypeRepository>();
builder.Services.AddTransient<IConstructionTypeRepository, ConstructionTypeRepository>();
builder.Services.AddTransient<IConstructionStageRepository, ConstructionStageRepository>();
builder.Services.AddTransient<ILandholdingPictureRepository, LandholdingPictureRepository>();
builder.Services.AddTransient<ILandholdingFeatureRepository, LandholdingFeatureRepository>();
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200");
    });
});

//DB
builder.Services.AddDbContext<RealEstaterDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnectionString"));
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = false,
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
