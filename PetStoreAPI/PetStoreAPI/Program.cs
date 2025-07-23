using AutoMapper;
using MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetStoreAPI.Data;
using PetStoreAPI.Extensions;
using PetStoreAPI.Models;
using PetStoreAPI.Service;
using PetStoreAPI.Service.IService;
using PetStoreAPI.Configurations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog'u Seq ile yapýlandýr
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration) // appsettings.json'dan okur
        .Enrich.FromLogContext()
        .WriteTo.Seq("http://localhost:5341"); // Seq sunucusunun URL'si
});

// Diðer servis kayýtlarý
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

// Add services to the container.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: 'Bearer Generated-JWT-Token'",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] { }
        }
    });
});
builder.AddAppAuthetication();
builder.Services.AddAuthorization();

// CORS ayarlarý
builder.Services.AddCors(options =>
{
    // Local geliþtirme ortamý için izin verilecek adresler
    options.AddPolicy("LocalDevelopmentCors", policy =>
    {
        policy.WithOrigins(
                "https://localhost:7251",
                "http://localhost:5294",
                "http://localhost:35160",
                "https://localhost:44319"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });

    // Yayýn ortamýnda izin verilecek adresler
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins(
                "https://paficapiv2.justkey.online/",
                "https://paficapi.justkey.online/",
                "https://petstoreapi.justkey.online"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();

// Ortama göre doðru CORS politikasýný uygula
if (app.Environment.IsDevelopment())
{
    app.UseCors("LocalDevelopmentCors");
}
else
{
    app.UseCors("AllowSpecificOrigin");
}

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();
app.Run();
