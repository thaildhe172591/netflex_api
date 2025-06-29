using Netflex.Application.UseCases.V1.Keywords.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Keywords;

public record CreateKeywordRequest(string Name);
public record CreateKeywordResponse(long Id);

public class CreateKeywordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/keywords", async (CreateKeywordRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateKeywordCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result.Adapt<CreateKeywordResponse>());
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(CreateKeywordEndpoint));
    }
}