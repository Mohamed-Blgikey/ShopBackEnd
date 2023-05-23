using API.Helper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("def")!);
});

builder.Services.AddCors();

builder.Services.AddScoped<IproductRepo, ProductRepo>();
builder.Services.AddScoped<IBasketRep, BasketRep>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    ConfigurationOptions opt = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis")!);
    return ConnectionMultiplexer.Connect(opt);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(a=>a.AllowAnyHeader().SetIsOriginAllowed(a=>true).AllowAnyMethod());
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    logger.LogError(ex, "error migration");
}
app.Run();
