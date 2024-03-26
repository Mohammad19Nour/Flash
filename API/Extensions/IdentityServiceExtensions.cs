using ProjectP.Data;
using ProjectP.Data.Entities.Identity;

namespace ProjectP.Extensions;

using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthorization(opts => { opts.AddPolicy("Driver_Role", policy => policy.RequireRole("Driver")); });
        services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
            }).AddRoles<AppRole>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoleValidator<RoleValidator<AppRole>>()
            .AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
                opt.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userManager = context.HttpContext.RequestServices
                            .GetRequiredService<UserManager<AppUser>>();
                        var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;
                        if (claimsIdentity is null)
                            context.Fail("Unauthorized");
                        else
                        {
                            var email = claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value;

                            
                            if (email is null)
                            {
                                context.Fail("Unauthorized");
                            }
                            else
                            {
                                var user = await userManager.Users.FirstOrDefaultAsync(x=>x.Email.ToLower() == email.ToLower());
                                if (user == null)
                                {
                                    context.Fail("Unauthorized");
                                }
                                else
                                {
                                    
                                    var roles = await userManager.GetRolesAsync(user);

                                    // Add the custom claim to the bearer token
                                    var identity = new ClaimsIdentity();
                                    identity.AddClaims(roles.Select(r => new Claim(ClaimTypes.Role, r)));
                                    context.Principal?.AddIdentity(identity);
                                }

                            }
                        }
                    },
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        return services;
    }
}