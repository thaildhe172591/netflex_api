using Netflex.Application.UseCases.V1.Series.Queries;

namespace Netflex.WebAPI.Endpoints.V1.Series;

public record GetSerieRequest(long Id);
public class GetSerieEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/series/{id}", async ([AsParameters] GetSerieRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.Adapt<GetSerieQuery>());
            return Results.Ok(result);
        })
        // .RequireAuthorization("EmailVerified")
        .MapToApiVersion(1)
        .WithName(nameof(GetSerieEndpoint));
    }
}