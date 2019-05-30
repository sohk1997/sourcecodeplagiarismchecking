using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Root.Data;
using Root.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Configuration
{
    public static class IdentityConfigure
    {
        public static void AddIdentityConfigure(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(op => {
                op.Password.RequireNonAlphanumeric = false;
                op.Password.RequireUppercase = false;
                op.Password.RequireDigit = true;
                op.Password.RequiredLength = 6;
                op.Password.RequireLowercase = true;
            })
           .AddEntityFrameworkStores<TestContext>()
           .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;
                option.SaveToken = true;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecrectKeyAhihiBleBle")),
                    ValidIssuer = "Test",
                    ValidAudience = "Test",
                };
            });

            services.AddAuthorization(options => options.AddPolicy("Luat", policy => policy.RequireClaim("luat")));
        }
    }
}
