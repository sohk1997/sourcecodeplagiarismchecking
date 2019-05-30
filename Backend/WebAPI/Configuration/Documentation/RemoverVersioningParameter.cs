using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Configuration
{
    public class RemoverVersioningParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters != null && operation.Parameters.Count > 0)
            {
                var versionParameter = operation.Parameters.SingleOrDefault(s => s.Name == null ? false : s.Name.Contains("ver"));
                operation.Parameters.Remove(versionParameter);
            }
        }
    }
}
