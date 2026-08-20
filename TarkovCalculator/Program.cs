using Microsoft.EntityFrameworkCore;
using Tarkov.API.Services;
using Tarkov.Infrastructure.Data;
using TarkovCalculator.Components;
using TarkovCalculator.Protos;

var builder = WebApplication.CreateBuilder(args);

// 1. Database en API Services toevoegen
builder.Services.AddControllers();
builder.Services.AddDbContext<TarkovDbContext>(options =>
    options.UseSqlite("Data Source=tarkov.db"));

builder.Services.AddHttpClient<TarkovAPIService>();
builder.Services.AddScoped<TarkovSyncService>();

// 2. gRPC Server toevoegen
builder.Services.AddGrpc();

// 3. gRPC Client configureren die naar zichzelf luistert
// We gebruiken hier een vast lokale poort (bijv. 5000), zodat de app altijd lokaal kan verbinden
builder.Services.AddGrpcClient<TarkovService.TarkovServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5000");
});

// 4. Blazor componenten toevoegen
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// 5. Automatisch de database migreren en data synchroniseren bij opstarten
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TarkovDbContext>();
    await dbContext.Database.EnsureCreatedAsync(); // Of MigrateAsync() als jemigrations gebruikt

    var syncService = scope.ServiceProvider.GetRequiredService<TarkovSyncService>();
    await syncService.SyncDataAsync();
}

// 6. HTTP Request Pipeline configureren
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthorization();

// Map Controllers en gRPC services
app.MapControllers();
app.MapGrpcService<TarkovGrpcService>();

// Map Blazor componenten
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 7. Zorg dat de webserver luistert naar de poort waar de gRPC client naartoe praat
app.Run("http://localhost:5000");