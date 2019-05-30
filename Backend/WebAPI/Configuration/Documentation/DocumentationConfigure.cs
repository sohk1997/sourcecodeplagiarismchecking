using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Configuration
{
    public static class DocumentationConfigure
    {
        public static void AddDocumentationConfigure(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("1.0", new Info()
                {
                    Version = "1.0",
                    Title = "Energy.Api Help"
                });
                option.SwaggerDoc("2.0", new Info()
                {
                    Version = "2.0",
                    Title = "Energy.Api Help"
                });
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                option.AddSecurityRequirement(security);
                option.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                option.DocInclusionPredicate((documentName, apiDes) =>
                {
                    var versions = apiDes.ActionDescriptor.Properties.ToList()
                                                          .Where(x => x.Value.GetType() == typeof(ApiVersionModel))
                                                          .Select(x => (ApiVersionModel)x.Value)
                                                          .ToList();
                    return versions.Count == 0 ? true : versions.Any(x => x.ImplementedApiVersions.Any(y => y.MajorVersion + "." + y.MinorVersion == documentName));
                });
                option.OperationFilter<RemoverVersioningParameter>();
                option.OperationFilter<AddVersioningToOperationId>();
                option.DocumentFilter<SetVersionInPath>();
                var xmlDocPath = PlatformServices.Default.Application.ApplicationBasePath;
                option.IncludeXmlComments(Path.Combine(xmlDocPath, "Energy.Api.Help.xml"));
            });
        }
    }
}
