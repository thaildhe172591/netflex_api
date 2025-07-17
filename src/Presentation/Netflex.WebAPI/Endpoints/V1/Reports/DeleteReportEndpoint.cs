using Netflex.Application.UseCases.V1.Reports.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Reports;

public record DeleteReportRequest(long Id);

public class DeleteReportEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/reports/{id}", async ([AsParameters] DeleteReportRequest request, ISender sender) =>
        {
            await sender.Send(request.Adapt<DeleteReportCommand>());
            return Results.Ok();
        })
        .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(DeleteReportEndpoint));
    }
}