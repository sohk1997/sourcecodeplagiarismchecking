using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Root.Data;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Service.Services;
using Swashbuckle.AspNetCore;
using WebAPI.Configuration;
using WebAPI.Configuration.Filter;
using WebAPI.SignalRHub;

namespace WebAPI {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            //Add DB context
            services.AddDbContext<TestContext> (op => op.UseSqlServer (Configuration.GetConnectionString ("TestDB")));

            //Add identity
            services.AddIdentityConfigure ();

            //Add injection
            services.AddInjection ();

            //Add auto migration
            services.AddMigrationConfigure (Configuration);

            services.AddCors (options => {
                options.AddPolicy ("AllowAnyOrigins", builder => builder.AllowAnyOrigin ().AllowAnyHeader ().AllowAnyMethod ().AllowCredentials ());
            });

            //Add versioning config
            services.AddVersionConfigure ();

            //Add documentation config
            services.AddDocumentationConfigure ();

            //Add auto mapper config
            services.AddAutoMapper ();

            services.AddMvc (option => option.Filters.Add (typeof (ExceptionFilter)));

            services.AddSignalR ();

            services.Configure<MvcOptions> (options => {
                options.Filters.Add (new CorsAuthorizationFilterFactory ("AllowAnyOrigins"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseCors ("AllowAnyOrigins");
            app.UseAuthentication ();
            app.UseMvc ();

            app.UseSwagger ();
            app.UseSwaggerUI (options => {
                options.SwaggerEndpoint (
                    "/swagger/1.0/swagger.json", "Version 1.0");
                options.SwaggerEndpoint (
                    "/swagger/2.0/swagger.json", "Version 2.0");
            });

            app.UseSignalR (routes => {
                routes.MapHub<TestHub> ("/testHub");
            });
        }
    }
}