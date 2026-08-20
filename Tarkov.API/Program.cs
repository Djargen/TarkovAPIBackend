using Tarkov.Infrastructure.DTOResponses;
using Tarkov.API.Services;
using Tarkov.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<TarkovDbContext>(options => options.UseSqlite("Data Source=tarkov.db"));
builder.Services.AddHttpClient<TarkovAPIService>();
builder.Services.AddScoped<TarkovSyncService>();

// Add gRPC services
builder.Services.AddGrpc();

var app = builder.Build();

// Zorg ervoor dat de data wordt gesynchroniseerd zodra de app start:
using (var scope = app.Services.CreateScope())
{
    var syncService = scope.ServiceProvider.GetRequiredService<TarkovSyncService>();
    await syncService.SyncDataAsync();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Map gRPC service
app.MapGrpcService<TarkovGrpcService>();

app.Run();