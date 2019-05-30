using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace WebAPI.Configuration
{
    public static class VersionConfigure
    {
        public static void AddVersionConfigure(this IServiceCollection services)
        {
            services.AddApiVersioning(op =>
            {
                op.ReportApiVersions = true;
                op.AssumeDefaultVersionWhenUnspecified = true;
                op.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                var controllerClass = Assembly.GetAssembly(typeof(VersionConfigure))
                                          .GetExportedTypes()
                                          .Where(s => s.Name.Contains("Controller"))
                                          .Where(s => !s.IsInterface)
                                          .ToList();

                //Add controller Version 1
                foreach (var controller in controllerClass.Where(s => s.FullName.Contains(".V1.")))
                {
                    op.Conventions.Controller(controller).HasApiVersion(new Microsoft.AspNetCore.Mvc.ApiVersion(1,0));
                }

                //Add controller Version 2
                foreach (var controller in controllerClass.Where(s => s.FullName.Contains(".V2.")))
                {
                    op.Conventions.Controller(controller).HasApiVersion(new Microsoft.AspNetCore.Mvc.ApiVersion(2, 0));
                }

                //Add controller Version 2
                foreach (var controller in controllerClass.Where(s => !s.FullName.Contains(".V2.") && !s.FullName.Contains(".V1.")))
                {
                    op.Conventions.Controller(controller).HasApiVersion(new Microsoft.AspNetCore.Mvc.ApiVersion(2, 0));
                    op.Conventions.Controller(controller).HasApiVersion(new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0));
                }
            }
            );
        }
    }
}
