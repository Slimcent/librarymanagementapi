using Library.API.Action_Filters;
using Library.Entities.Entities;
using Library.LoggerService;
using Library.Repository.Implementation;
using Library.Repository.Interfaces;
using Library.Services.Implementation;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Middleware
{
    namespace Library.API.Middlewares
    {
        public static class ServiceExtensions
        {
            public static void ConfigureCors(this IServiceCollection services) =>
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                });

            public static void ConfigureIISIntegration(this IServiceCollection services) =>
                services.Configure<IISOptions>(options =>
                {
                });

            public static void ConfigureIdentity(this IServiceCollection services)
            {
                services.AddIdentity<User, Role>(o =>
                {
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 5;
                    o.User.RequireUniqueEmail = true;
                }).AddEntityFrameworkStores<LibraryContext>().AddDefaultTokenProviders();
            }

            public static IServiceCollection AddDBConnection(this IServiceCollection services,
                IConfiguration configuration)
            {
                services.AddDbContext<LibraryContext>(opts =>
                    opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                    b => b.MigrationsAssembly("Library.API")));

                return services;
            }

            public static IServiceCollection ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
            {
                services.AddTransient<DbContext, LibraryContext>();
                services.AddTransient<IUnitofWork, UnitofWork<LibraryContext>>();
                services.AddTransient<IBookService, BookService>();
                services.AddTransient<IUserService, UserService>();
                services.AddTransient<IAuthorService, AuthorService>();
                services.AddTransient<ILoggerManager, LoggerManager>();

                return services;
            }

            public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
            {
                var jwtSettings = configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings.GetSection("Secret").Value;
                services.AddAuthentication(opt => {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                        ValidAudience = jwtSettings.GetSection("validAudience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });
            }

            public static void ConfigureActionFilters(this IServiceCollection services)
            {
                services.AddScoped<AdminControllerValidation>();
            }
        }
    }
}