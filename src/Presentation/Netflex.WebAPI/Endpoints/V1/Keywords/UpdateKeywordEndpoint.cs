using Microsoft.AspNetCore.Mvc;
using Netflex.Application.UseCases.V1.Keywords.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Keywords;

public record UpdateKeywordRequest(string Name);
public class UpdateKeywordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/keywords/{id}", async (long id, [FromBody] UpdateKeywordRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateKeywordCommand>() with { Id = id };
            await sender.Send(command);
            return Results.Ok();
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(UpdateKeywordEndpoint));
    }
}