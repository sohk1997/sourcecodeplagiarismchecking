using Microsoft.AspNetCore.Mvc.Versioning;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Configuration
{
    public class AddVersioningToOperationId : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var apiDes = context.ApiDescription;
            string action = apiDes.ActionDescriptor.DisplayName;
            action = action.Replace("(WebAPI)", "");
            string []actions = action.Split(".");

            var versions = apiDes.ActionDescriptor.Properties.ToList()
                                                      .Where(x => x.Value.GetType() == typeof(ApiVersionModel))
                                                      .Select(x => (ApiVersionModel)x.Value)
                                                      .ToList();
            operation.OperationId = actions[actions.Length - 2] + actions[actions.Length - 1]; 
        }
    }
}
