using Netflex.Domain.Events;

namespace Netflex.Application.UseCases.V1.Episode.EventHandlers;

public class EpisodeCreatedEventHandler(ILogger<EpisodeCreatedEventHandler> logger, ISender sender)
        : INotificationHandler<EpisodeCreatedEvent>
{
    private readonly ILogger<EpisodeCreatedEventHandler> _logger = logger;
    private readonly ISender _sender = sender;

    public async Task Handle(EpisodeCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event handled: {DomainEvent}", nameof(EpisodeCreatedEvent));
        await Task.CompletedTask;
    }
}