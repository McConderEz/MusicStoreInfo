using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
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


        public static bool DoesRoleExist(string connectionString, string roleName)
        {
            string checkRoleSql = $@"
                SELECT COUNT(*) 
                FROM musicStores 
                WHERE type = 'R' AND name = @roleName";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(checkRoleSql, connection))
                    {
                        command.Parameters.AddWithValue("@roleName", roleName);
                        int roleCount = (int)command.ExecuteScalar();
                        return roleCount > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return false;
                }
            }
        }

        public static void CreateRole(string connectionString, string roleName)
        {
            string createRoleSql = $@"
                    CREATE ROLE {roleName};";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(createRoleSql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"Role '{roleName}' created successfully.");
                    }

                    string getPrincipalIdSql = $@"
                             SELECT principal_id 
                             FROM sys.database_principals 
                             WHERE name = @roleName";

                    using(SqlCommand command = new SqlCommand(getPrincipalIdSql, connection))
                    {
                        command.Parameters.AddWithValue("@roleName", roleName);
                        var principalId = (int)command.ExecuteScalar();

                        var role = new Role
                        {
                            Name = roleName,
                            PrincipalId = principalId
                        };

                        SaveRoleToDatabase(role);
                    }

                }                
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        private static void SaveRoleToDatabase(Role role)
        {
            using (var context = new MusicStoreDbContext())
            {
                context.Roles.Add(role);
                context.SaveChanges();
            }
        }

        public static void EnsureRolesExist(this IServiceCollection services,string connectionString, string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (!DoesRoleExist(connectionString, roleName))
                {
                    CreateRole(connectionString, roleName);
                }
                else
                {
                    Console.WriteLine($"Role '{roleName}' already exists.");
                }
            }
        }
    }
}
