using Netflex.Shared.Pagination;

namespace Netflex.WebAPI.Endpoints.V1.Keywords;

public record GetKeywordsRequest(string? Query, string? OrderBy) : PaginationRequest;
public record GetKeywordsResponse(PaginatedResult<string> Keywords);

public class GetKeywordsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/keywords", ([AsParameters] GetKeywordsRequest request) =>
        {
            return Results.Ok(request);
        })
        .MapToApiVersion(1)
        .WithName(nameof(GetKeywordsEndpoint));
    }
}