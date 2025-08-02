using APIVault.API.Data;
using APIVault.API.Helpers;
using APIVault.API.Services;
using APIVault.API.Services.Implementations;
using APIVault.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//  Configure Database Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//  Enable CORS
builder.Services.AddCors();

//  Dependency Injection
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<EncryptionHelper>();
builder.Services.AddScoped<JwtHelper>();

//  Add Controllers
builder.Services.AddControllers();

//  Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//  Enable CORS Globally
app.UseCors(options =>
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

//  Enable Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "APIVault API V1");
    options.RoutePrefix = ""; // Swagger will open at http://localhost:5000/
});

//  HTTPS Redirection
app.UseHttpsRedirection();

//  Authorization
app.UseAuthorization();

//  Map Controllers
app.MapControllers();

//  Run the App
app.Run();
