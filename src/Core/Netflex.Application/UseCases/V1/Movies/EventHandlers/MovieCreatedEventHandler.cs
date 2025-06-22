using Netflex.Domain.Events;

namespace Netflex.Application.UseCases.V1.Movies.EventHandlers;

public class MovieCreatedEventHandler(ILogger<MovieCreatedEventHandler> logger, ISender sender)
        : INotificationHandler<MovieCreatedEvent>
{
    private readonly ILogger<MovieCreatedEventHandler> _logger = logger;
    private readonly ISender _sender = sender;

    public async Task Handle(MovieCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event handled: {DomainEvent}", nameof(MovieCreatedEvent));
        await Task.CompletedTask;
    }
}