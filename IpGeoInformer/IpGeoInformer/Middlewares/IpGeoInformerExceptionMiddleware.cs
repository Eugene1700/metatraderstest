using System;
using System.Net;
using System.Threading.Tasks;
using IpGeoInformer.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IpGeoInformer.Middlewares
{
    public class IpGeoInformerExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IpGeoInformerExceptionMiddleware> _logger;
        
        public IpGeoInformerExceptionMiddleware(RequestDelegate next, ILogger<IpGeoInformerExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (DomainException domainException)
            {
                await HandleDomainExceptionAsync(httpContext, domainException);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Необработанное исключение. Метод {Method}", context.Request.Path);
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new {error = "Произошла непредвиденная ошибка при работе сервиса. Обратитесь к разработчикам"});
            await context.Response.WriteAsync(result);
        }
        
        private async Task HandleDomainExceptionAsync(HttpContext context, DomainException exception)
        {
            _logger.LogWarning(exception, "Ошибки уровня бизнес логики");
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            
            var result = JsonConvert.SerializeObject(new
            {
                error = exception.Message,
                type = nameof(DomainException)
            });
            await context.Response.WriteAsync(result);
        }
    }
}