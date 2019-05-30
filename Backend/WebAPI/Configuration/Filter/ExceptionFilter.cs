using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebAPI.Configuration.Filter {
    public class ExceptionFilter : IExceptionFilter {
        IHostingEnvironment _env;
        ILogger<ExceptionFilter> _logger;
        public ExceptionFilter (IHostingEnvironment env, ILogger<ExceptionFilter> logger) {
            _env = env;
            _logger = logger;
        }
        public void OnException (ExceptionContext context) {
            if (!_env.IsDevelopment ()) {
                var exception = context.Exception;
                while (exception != null) {
                    _logger.LogError (exception.Message);
                    _logger.LogError (exception.StackTrace);
                    exception = exception.InnerException;
                }
                context.Exception = null;
                context.Result = new StatusCodeResult (500);
            }
        }
    }
}