using global::ApptManager.Middleware;
using Microsoft.AspNetCore.Builder;

namespace ApptManager.GlobalException
{

        public static class ExceptionMiddlewareExtensions
        {
            public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
            {
                return app.UseMiddleware<ExceptionMiddleware>();
            }
        }
    

}
