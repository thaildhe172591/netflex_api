using MassTransit;
using Netflex.Application.UseCases.V1.Auth.Commands;
using Netflex.Domain.Events;

namespace Netflex.Application.UseCases.V1.Users.EventHandlers;

public class UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger, IPublishEndpoint publishEndpoint)
        : INotificationHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> _logger = logger;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;


    public async Task Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event handled: {DomainEvent}", nameof(UserCreatedEvent));
        var command = new SendOtpCommand(domainEvent.User.Email.Value);
        await _publishEndpoint.Publish(command, cancellationToken);
    }
}