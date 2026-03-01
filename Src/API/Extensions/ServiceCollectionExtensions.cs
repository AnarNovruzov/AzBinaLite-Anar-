using API.Options;
using Application.Abstractions.Services;
using Application.Abstracts.Repositories;
using Application.Abstracts.Services;
using Application.Options;
using Application.Validations.Auth;
using Application.Validations.CityValidation;
using Application.Validations.PropertyAdValidation;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence.Context;
using Persistence.Repositories;
using Persistence.Services;

namespace API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();

        services.AddDbContext<BinaLiteDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


        //services.AddMinioStorage(configuration);
        //services.AddScoped<IFileStorageService, S3MinioFileStorageService>();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {


            c.CustomSchemaIds(t => t.FullName);

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header. Example: \"Bearer {token}\""
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
                }
            },
            Array.Empty<string>()
        }
    });
        });

        services.AddValidatorsFromAssemblyContaining<CreatePropertyAdValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdatePropertyAdValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateCityValidation>();
        services.AddValidatorsFromAssemblyContaining<UpdateCityValidation>();
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services
            .AddIdentity<User, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;

                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
            })
            .AddEntityFrameworkStores<BinaLiteDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));


        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IPropertyAdRepository, PropertyAdRepository>();
        
        services.AddScoped<IPropertyAdService, PropertyAdService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<ICityService, CityService>();

        services.AddScoped<IAuthService, AuthService>();

        services.ConfigureOptions<ConfigureJwtBearerOptions>();

         
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
        services.AddAuthorization();
        services.AddScoped<IEmailService, EmailService>();





        return services;
    }
}
