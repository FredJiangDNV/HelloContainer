namespace HelloContainer.Api.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseDomainExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DomainExceptionHandlerMiddleware>();
        }
    }
} 