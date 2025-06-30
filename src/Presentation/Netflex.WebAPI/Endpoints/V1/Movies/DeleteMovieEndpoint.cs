using Netflex.Application.UseCases.V1.Movies.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Movies;

public record DeleteMovieRequest(long Id);

public class DeleteMovieEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/movies/{id}", async ([AsParameters] DeleteMovieRequest request, ISender sender) =>
        {
            var command = request.Adapt<DeleteMovieCommand>();
            await sender.Send(command);
            return Results.Ok();
        })
        // .RequireAuthorization("EmailVerified")
        .MapToApiVersion(1)
        .WithName(nameof(DeleteMovieEndpoint));
    }
}