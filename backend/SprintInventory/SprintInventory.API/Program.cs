using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SprintInventory.Core.Interfaces;
using SprintInventory.Core.Interfaces.Repositories;
using SprintInventory.Core.Interfaces.Services.Entity;
using SprintInventory.Core.Interfaces.Services.Extension;
using SprintInventory.Core.Interfaces.Services.Mapper;
using SprintInventory.Core.Interfaces.Services.Security;
using SprintInventory.Core.Models.Settings;
using SprintInventory.Infrastructure;
using SprintInventory.Infrastructure.Repositories;
using SprintInventory.Services.EntityServices;
using SprintInventory.Services.Extensions;
using SprintInventory.Services.Mappers;
using SprintInventory.Services.Security;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"]
                                       ?? throw new ApplicationException("SecurityKey is missing."))
            )
        };
        opt.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token)) context.Token = token;
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                try
                {
                    var claims = context.Principal?.Claims;
                    var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    if (Guid.TryParse(userIdClaim?.Value, out var userId))
                    {
                        var identity = context.Principal.Identity as ClaimsIdentity;
                        if (identity != null)
                        {
                            if (!identity.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                            {
                                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to parse UserId from token");
                        context.Fail("Invalid UserId in token");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Token validation error: {ex.Message}");
                    context.Fail("Invalid token");
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(configuration.GetConnectionString("PostgresSQL"))
        .UseLazyLoadingProxies()
);
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICreatingLogRepository, CreatingLogRepository>();
builder.Services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
builder.Services.AddScoped<IMovementRepository, MovementRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IStatusLogRepository, StatusLogRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IInventoryItemService, InventoryItemService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IItemStatusExtensionService, ItemStatusExtensionService>();

builder.Services.AddScoped<ICategoryMapper, CategoryMapper>();
builder.Services.AddScoped<IInventoryItemMapper, InventoryItemMapper>();
builder.Services.AddScoped<ILogMapper, LogMapper>();
builder.Services.AddScoped<IRoomMapper, RoomMapper>();
builder.Services.AddScoped<IUserMapper, UserMapper>();

builder.Services.AddScoped<IHashService, Sha256Hasher>();
builder.Services.AddScoped<IJwtService>(opt =>
{
    var settings = new JwtSettings(
        Audience: configuration["JWT:Audience"] ?? throw new NullReferenceException("Audience"),
        Issuer: configuration["JWT:Issuer"] ?? throw new NullReferenceException("Issuer"),
        Secret: configuration["JWT:Secret"] ?? throw new NullReferenceException("Secret"),
        ExpirationHours: int.Parse(configuration["JWT:ExpirationHours"]
                                   ?? throw new NullReferenceException("ExpirationHours"))
    );
    return new JwtService(settings);
});


builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();