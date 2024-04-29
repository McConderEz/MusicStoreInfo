using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicStoreInfo.Infrastructure;
using System.Security.Claims;
using System.Text;

namespace MusicStoreInfo.Api.Extensions
{
    public static class ApiExtensions
    {

        public static void AddApiAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["$data-cookies"];

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "Admin");
                });

                options.AddPolicy("Manager", policy =>
                {
                    policy.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, "Admin")
                                                || x.User.HasClaim(ClaimTypes.Role, "Manager"));
                });

                options.AddPolicy("Client", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "Client");
                });
            });
        }
    }
}
