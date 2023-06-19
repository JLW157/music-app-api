using Newtonsoft.Json;

namespace MusicAppApi.API.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case ArgumentException:
                        {
                            break;
                        }
                };

                _logger.LogError(e, e.Message);

                var error = new CustomError
                {
                    Message = "An error occurred.",
                    Details = e.Message
                };


                var json = JsonConvert.SerializeObject(error);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(json);
            }
        }
    }
}

public class CustomError
{
    public string Message { get; set; }
    public string Details { get; set; }
}
