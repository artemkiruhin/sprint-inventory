using Microsoft.EntityFrameworkCore;
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

app.UseAuthorization();

app.MapControllers();

app.Run();