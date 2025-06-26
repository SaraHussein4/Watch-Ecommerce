using Microsoft.EntityFrameworkCore;
using Watch_Ecommerce.Helpers;
using ECommerce.Core.model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Watch_EcommerceBl.UnitOfWorks;
using Watch_EcommerceBl.Interfaces;
using Watch_Ecommerce.Services;

namespace Watch_Ecommerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddIdentity<User , IdentityRole>(Options =>
            {
                Options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<TikrContext>()
            .AddDefaultTokenProviders();
            ;
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            builder.Services.AddScoped<IUnitOfWorks , UnitOfWork>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.AddAuthentication(Options =>
            {

                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };

            });

            #region Database & User Identity
            builder.Services.AddDbContext<TikrContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            
            #endregion


            #region CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            #endregion

            var app = builder.Build();

            var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbContext = service.GetRequiredService<TikrContext>();
                await dbContext.Database.MigrateAsync();

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occured during migration");
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json", "v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
