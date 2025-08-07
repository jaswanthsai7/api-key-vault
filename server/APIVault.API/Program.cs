using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using APIVault.API.Data;
using APIVault.API.Helpers;
using APIVault.API.Services;
using APIVault.API.Services.Implementations;
using APIVault.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configure Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policy =>
    {
        policy.WithOrigins("https://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add Services (DI)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();

builder.Services.AddScoped<EncryptionHelper>();
builder.Services.AddScoped<JwtHelper>();

// JWT Settings
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
    };

    //  Add diagnostic logging
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["access_token"];

            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;

                //  Log when token is successfully read
                Console.WriteLine("JWT token found in cookie.");
            }
            else
            {
                // Log when token is missing
                Console.WriteLine("No JWT token found in cookie.");
            }

            return Task.CompletedTask;
        },

        OnAuthenticationFailed = context =>
        {
            //  Log reason for failure
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },

        OnTokenValidated = context =>
        {
            // Log token success (optional)
            Console.WriteLine("Token successfully validated.");
            return Task.CompletedTask;
        }
    };
});


// Add Controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

// Add Swagger with JWT Authorization
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "APIVault API",
        Version = "v1"
    });

    // Add JWT "Authorize" button
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and then your JWT.\nExample: Bearer abc.def.ghi"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "APIVault API V1");
    options.RoutePrefix = ""; // Swagger at root
});

// Enable Middleware
app.UseHttpsRedirection();
app.UseCors("AllowLocalhost3000");
app.UseAuthentication(); // JWT Auth
app.UseMiddleware<APIVault.API.Middlewares.ApiKeyMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
