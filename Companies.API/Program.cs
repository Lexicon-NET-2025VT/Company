using Companies.API.Extensions;
using Companies.Infrastructure.Data;
using Companies.Infrastructure.Repositories;
using Companies.Services;
using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Companies.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<CompaniesContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CompaniesContext") ?? throw new InvalidOperationException("Connection string 'CompaniesContext' not found.")));

            builder.Services.AddControllers(configure => configure.ReturnHttpNotAcceptable = true)
                .AddNewtonsoftJson()
                .AddApplicationPart(typeof(AssemblyReference).Assembly);
                // .AddXmlDataContractSerializerFormatters();


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            // builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            // builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            // builder.Services.AddScoped<IServiceManager, ServiceManager>();



            builder.Services.ConfigureServiceLayerServices();
            builder.Services.ConfigureRepositories();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtSettings = builder.Configuration.GetSection("JwtSettings");
                ArgumentNullException.ThrowIfNull(nameof(jwtSettings));

                var secretKey = builder.Configuration["secretkey"];
                ArgumentNullException.ThrowIfNull(secretKey, nameof(secretKey));

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateLifetime = true
                };
            });





            builder.Services.AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.Password.RequireLowercase = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 3;
                opt.User.RequireUniqueEmail = true;
            }
            ).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<CompaniesContext>()
            .AddDefaultTokenProviders();




            builder.Services.ConfigureCors();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                await app.SeedDataAsync();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
