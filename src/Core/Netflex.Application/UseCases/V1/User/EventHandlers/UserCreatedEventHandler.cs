using Netflex.Application.UseCases.V1.Auth.Commands;
using Netflex.Application.UseCases.V1.User.Commands;
using Netflex.Domain.Events;

namespace Netflex.Application.UseCases.V1.User.EventHandlers;

public class UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger, ISender sender)
        : INotificationHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> _logger = logger;
    private readonly ISender _sender = sender;

    private const string DEFAULT_ROLE_NAME = "User";

    public async Task Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event handled: {DomainEvent}", nameof(UserCreatedEvent));
        await _sender.Send(new AssignRoleCommand(domainEvent.User.Id, DEFAULT_ROLE_NAME), cancellationToken);
        await _sender.Send(new SendOTPCommand(domainEvent.User.Email.Value), cancellationToken);
    }
}