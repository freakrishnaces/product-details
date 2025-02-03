public class CatchApplicationLevelExceptions
{
    private readonly RequestDelegate _next;

    public CatchApplicationLevelExceptions(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            error = new
            {
                message = exception.Message,
                type = exception.GetType().Name
            }
        };

        switch (exception)
        {
            case KeyNotFoundException or NullReferenceException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                break;
            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        await context.Response.WriteAsJsonAsync(response);
    }
}