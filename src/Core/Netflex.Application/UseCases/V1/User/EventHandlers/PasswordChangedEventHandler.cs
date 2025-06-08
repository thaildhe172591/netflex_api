using Netflex.Application.UseCases.V1.Auth.Commands;
using Netflex.Domain.Events;

namespace Netflex.Application.UseCases.V1.User.EventHandlers;

public class PasswordChangedEventHandler(ILogger<PasswordChangedEventHandler> logger, ISender sender)
        : INotificationHandler<PasswordChangedEvent>
{
    private readonly ILogger<PasswordChangedEventHandler> _logger = logger;
    private readonly ISender _sender = sender;

    public async Task Handle(PasswordChangedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event handled: {DomainEvent}", nameof(PasswordChangedEvent));
        await _sender.Send(new RevokeAllExceptCommand(domainEvent.User.Id, domainEvent.SessionId), cancellationToken);
    }
}