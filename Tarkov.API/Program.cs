using Tarkov.Infrastructure.DTOResponses;
using Tarkov.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DbContext>(options => options.UseSqlite("Data Source=tarkov.db"));


builder.Services.AddHttpClient<TarkovAPIService>();

// Add gRPC services
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Map gRPC service
app.MapGrpcService<TarkovGrpcService>();

app.Run();
