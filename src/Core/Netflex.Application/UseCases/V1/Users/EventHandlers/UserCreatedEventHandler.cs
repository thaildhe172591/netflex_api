using Netflex.Application.UseCases.V1.Auth.Commands;
using Netflex.Domain.Events;

namespace Netflex.Application.UseCases.V1.Users.EventHandlers;

public class UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger, ISender sender)
        : INotificationHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> _logger = logger;
    private readonly ISender _sender = sender;

    public async Task Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event handled: {DomainEvent}", nameof(UserCreatedEvent));
        await _sender.Send(new SendOTPCommand(domainEvent.User.Email.Value), cancellationToken);
    }
}