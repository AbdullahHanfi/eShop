using System.Net;
using System.Text.Json;

namespace Middlewares;

public class ErrorHandlingMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IWebHostEnvironment environment) {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context) {
        try{ await _next(context); }
        catch (Exception ex){
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception) {
        var isApiRequest = context.Request.Path.StartsWithSegments("/api") ||
                           context.Request.Headers["Accept"].ToString().Contains("application/json") ||
                           context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        if (isApiRequest){ await HandleApiExceptionAsync(context, exception); }
        else{ await HandleMvcExceptionAsync(context, exception); }
    }

    private async Task HandleApiExceptionAsync(HttpContext context, Exception exception) {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse();

        switch (exception){
            case ValidationException:
                response.Message = exception.Message;
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case NotFoundException:
                response.Message = exception.Message;
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case UnauthorizedException:
                response.Message = exception.Message;
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;
            default:
                response.Message = _environment.IsDevelopment()
                    ? exception.Message
                    : "An error occurred while processing your request.";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        if (_environment.IsDevelopment()){ response.Details = exception.StackTrace; }

        var jsonResponse = JsonSerializer.Serialize(response,
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    private async Task HandleMvcExceptionAsync(HttpContext context, Exception exception) {
        switch (exception){
            case ValidationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case NotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case UnauthorizedException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        context.Response.Redirect($"/Error/{context.Response.StatusCode}");
    }
}

public class ErrorResponse {
    public string Message { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string TraceId { get; set; } = Guid.NewGuid().ToString();
    public string Details { get; set; }
}

public class ValidationException : Exception {
    public ValidationException(string message) : base(message) {}
}

public class NotFoundException : Exception {
    public NotFoundException(string message) : base(message) {}
}

public class UnauthorizedException : Exception {
    public UnauthorizedException(string message = "Unauthorized access") : base(message) {}
}