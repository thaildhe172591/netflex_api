using Microsoft.AspNetCore.Http;

namespace Netflex.Shared.Filters;

internal abstract record Response(bool Success);
internal record Failure(object? Error = null) : Response(false);
internal record Success(object? Result = null) : Response(true);

public class CustomResponseFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var result = await next(context);
        if (result is Response) return result;
        return new Success(result);
    }
}