using Microsoft.AspNetCore.Mvc;
using Netflex.Application.UseCases.V1.Reports.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Reports;

public record UpdateReportRequest(string? Reason, string? Description);
public class UpdateReportEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/reports/{id}", async (long id, [FromBody] UpdateReportRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateReportCommand>() with { Id = id };
            await sender.Send(command);
            return Results.Ok();
        })
        // .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(UpdateReportEndpoint));
    }
}