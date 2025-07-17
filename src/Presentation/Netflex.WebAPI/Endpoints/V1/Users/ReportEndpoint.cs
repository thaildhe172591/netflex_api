using Netflex.Application.UseCases.V1.Reports.Commands;

namespace Netflex.WebAPI.Endpoints.V1.Users;

public record ReportRequest(string Reason, string? Description);

public class ReportEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/report", async (ReportRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.Adapt<CreateReportCommand>());
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .MapToApiVersion(1)
        .WithName(nameof(ReportEndpoint));
    }
}