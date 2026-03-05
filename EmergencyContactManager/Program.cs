using Application;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using EmergencyContactManager.Components;
using EmergencyContactManager.Factories;
using EmergencyContactManager.Middlewares;
using Infrastructure;
using Infrastructure.Parsers;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContext>(options =>
    options.UseNpgsql(connectionString));
//dotnet ef migrations add InitialCreate --project infrastructure --startup-project EmergencyContactManager
//dotnet ef database update
//dotnet ef database drop --project Infrastructure --startup-project EmergencyContactManager --force

builder.Services.AddScoped<IApplicationTransaction, ApplicationTransaction>();
builder.Services.AddScoped<ICreateContactCommandFactory, CreateContactCommandFactory>();

builder.Services.AddScoped<IParsingService, CSVParser>();
builder.Services.AddScoped<IParsingService, JasonParser>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
DI.AddDI(builder.Services);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLocalization();
var app = builder.Build();

var supportedCultures = new[] { "ko-KR", "en-US" };
app.UseRequestLocalization(new RequestLocalizationOptions()
    .SetDefaultCulture("ko-KR")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures));


app.UseMiddleware<ApiExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else 
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Swagger UI 실행    
}

using (var scope = app.Services.CreateScope())
{
    var appDB = scope.ServiceProvider.GetRequiredService<DBContext>();
    await appDB.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStaticFiles();
app.UseAntiforgery();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

