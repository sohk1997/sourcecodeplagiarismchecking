using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Root.Data;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebAPI.Configuration
{
    public static class InjectionCofigure
    {
        public static void AddInjection(this IServiceCollection services)
        {
            var serviceClasses = Assembly.GetAssembly(typeof(IDocumentService))
                                          .GetExportedTypes()
                                          .Where(s => s.Name.EndsWith("Service"))
                                          .Where(s => !s.IsInterface)
                                          .ToList();
            var repositoryClassess = Assembly.GetAssembly(typeof(ISourceCodeRepository))
                                             .GetExportedTypes()
                                             .Where(s => s.Name.EndsWith("Repository"))
                                             .Where(s => !s.IsInterface)
                                             .ToList();
            foreach (var serviceClass in serviceClasses)
            {
                if (serviceClass.GetInterfaces().Where(s => s.Name == "I" + serviceClass.Name).FirstOrDefault() != null)
                {
                    services.Add(new ServiceDescriptor(serviceClass.GetInterfaces().Where(s => s.Name == "I" + serviceClass.Name).FirstOrDefault(), serviceClass, ServiceLifetime.Scoped));
                }
            }
            foreach (var repositoryClass in repositoryClassess)
            {
                if (repositoryClass.GetInterfaces().Where(u => u.Name == "I" + repositoryClass.Name).FirstOrDefault() != null)
                {
                    services.Add(new ServiceDescriptor(repositoryClass.GetInterfaces().Where(u => u.Name == "I" + repositoryClass.Name).FirstOrDefault(), repositoryClass, ServiceLifetime.Scoped));
                }
            }
            services.Add(new ServiceDescriptor(typeof(IUnitOfWork), typeof(UnitOfWork), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IDatabaseFactory), typeof(DatabaseFactory), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(DbContext), typeof(TestContext), ServiceLifetime.Scoped));
        }
    }
}
