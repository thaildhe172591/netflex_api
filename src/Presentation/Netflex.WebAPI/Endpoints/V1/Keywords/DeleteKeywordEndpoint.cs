using Netflex.Application.UseCases.V1.Keywords.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Keywords;

public record DeleteKeywordRequest(long Id);
public class DeleteKeywordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/keywords/{id}", async ([AsParameters] DeleteKeywordRequest request, ISender sender) =>
        {
            var command = request.Adapt<DeleteKeywordCommand>();
            await sender.Send(command);
            return Results.Ok();
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(DeleteKeywordEndpoint));
    }
}