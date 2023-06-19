using MusicAppApi.API.Middlewares;

namespace MusicAppApi.API.Extensions
{
    public static class AppExtensions
    {
        public static void UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}
