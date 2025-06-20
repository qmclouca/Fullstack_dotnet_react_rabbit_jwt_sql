using CrossCutting;
using Data;
using Data.Repositories;
using Domain.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;



builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.MSSqlServer(
        connectionString: context.Configuration.GetConnectionString("DefaultConnection"),
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        }));

var configuration = builder.Configuration;
builder.Services.AddDbContext<BaseApiDbContext>(options =>
    options.UseSqlServer
        (configuration.GetConnectionString("DefaultConnection")
        , b => b.MigrationsAssembly("../Data")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
#pragma warning disable CS8600, CS8603
builder.Services.AddScoped<IBaseApiDbContext>(provider => (IBaseApiDbContext)provider.GetService<BaseApiDbContext>());
#pragma warning restore CS8600, CS8603
builder.Services.AddApplicationServices();
//builder.Services.Configure<ExternalAPIConfigurations>(builder.Configuration.GetSection("ExternalAPIConfigurations"));
//builder.Services.AddTransient<IInfrastructureService, InfrastructureService>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            ValidateIssuer = false,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateAudience = false,
            ValidAudience = configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "BaseApiAPI", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autentica��o JWT usando o esquema Bearer. Exemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mercado F�cil API v1"));
    app.UseAuthentication();
    app.UseAuthorization();
}
else
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

#if !DEBUG
    app.UseAuthentication();
    app.UseAuthorization();
#endif

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();