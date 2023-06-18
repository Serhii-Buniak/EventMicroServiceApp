using CityMicroService.BLL;
using CityMicroService.DAL;
using CityMicroService.WebApi.StartupExtensions;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using CityMicroService.WebApi.GrpcControllers;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddFluentValidationAutoValidation();
services.AddValidatorsFromAssembly(typeof(AssemblyEntryPoint).Assembly);

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddAutoMapper(typeof(AssemblyEntryPoint).Assembly);
services.AddMemoryCache();

services.AddServicesList();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,

        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
});


const string corsName = "city-micro-service";
services.AddCors(options =>
{
    options.AddPolicy(corsName,
    corsbuilder =>
    {
        corsbuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin();
    });
});

services.AddGrpc();

var app = builder.Build();

app.UseCors(corsName);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<CityGrpcController>();
app.MapGet("/protos/city.server.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("../CityMicroService.BLL/Protos/city.server.proto"));
});

PrepDb.PrepPopulation(builder);

app.Run();
