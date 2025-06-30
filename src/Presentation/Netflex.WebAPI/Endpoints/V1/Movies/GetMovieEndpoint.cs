using Netflex.Application.UseCases.V1.Movies.Queries;

namespace Netflex.WebAPI.Endpoints.V1.Movies;

public record GetMovieRequest(long Id);
public class GetMovieEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/movies/{id}", async ([AsParameters] GetMovieRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.Adapt<GetMovieQuery>());
            return Results.Ok(result);
        })
        // .RequireAuthorization("EmailVerified")
        .MapToApiVersion(1)
        .WithName(nameof(GetMovieEndpoint));
    }
}