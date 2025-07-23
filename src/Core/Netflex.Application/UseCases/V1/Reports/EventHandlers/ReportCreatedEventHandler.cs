using MassTransit;
using Netflex.Application.UseCases.V1.Users.Commands;
using Netflex.Domain.Entities;
using Netflex.Domain.Events;

namespace Netflex.Application.UseCases.V1.Reports.EventHandlers;

public class ReportCreatedEventHandler(ILogger<ReportCreatedEventHandler> logger, IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork)
        : INotificationHandler<ReportCreatedEvent>
{
    private readonly ILogger<ReportCreatedEventHandler> _logger = logger;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;


    public async Task Handle(ReportCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event handled: {DomainEvent}", nameof(ReportCreatedEvent));
        var admins = await _unitOfWork.Repository<User>()
            .GetAllAsync(u => u.Roles.Any(r => r.Name == "Admin"), includeProperties: ["Roles"], cancellationToken: cancellationToken);
        foreach (var admin in admins)
        {
            var command = new SendMailCommand
            {
                To = admin.Email.Value,
                Subject = "New Report Created",
                Body = $"A new report has been created with ID: {domainEvent.Report.Id}."
            };

            await _publishEndpoint.Publish(command, cancellationToken);
        }
        await Task.CompletedTask;
    }
}