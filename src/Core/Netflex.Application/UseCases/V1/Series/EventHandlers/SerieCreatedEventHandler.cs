using Netflex.Domain.Events;

namespace Netflex.Application.UseCases.V1.Series.EventHandlers;

public class SerieCreatedEventHandler(ILogger<SerieCreatedEventHandler> logger, ISender sender)
        : INotificationHandler<TVSerieCreatedEvent>
{
    private readonly ILogger<SerieCreatedEventHandler> _logger = logger;
    private readonly ISender _sender = sender;

    public async Task Handle(TVSerieCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event handled: {DomainEvent}", nameof(TVSerieCreatedEvent));
        await Task.CompletedTask;
    }
}