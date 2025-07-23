using MassTransit;
using MassTransit.Initializers;
using Netflex.Application.UseCases.V1.Notifications.Commands;
using Netflex.Application.UseCases.V1.Users.Commands;
using Netflex.Domain.Entities;
using Netflex.Domain.Events;
using Netflex.Domain.ValueObjects;

namespace Netflex.Application.UseCases.V1.Episodes.EventHandlers;

public class EpisodeCreatedEventHandler(ILogger<EpisodeCreatedEventHandler> logger, ISender sender, IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork)
        : INotificationHandler<EpisodeCreatedEvent>
{
    private readonly ILogger<EpisodeCreatedEventHandler> _logger = logger;
    private readonly ISender _sender = sender;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(EpisodeCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event handled: {DomainEvent}", nameof(EpisodeCreatedEvent));

        var seriesId = domainEvent.Episode.SeriesId;
        var serie = _unitOfWork.Repository<TVSerie>().Get(seriesId);
        if (serie == null) return;

        var follows = await _unitOfWork.Repository<Follow>()
            .GetAllAsync(un => un.TargetId == seriesId.ToString() && un.TargetType == TargetType.TVSerie, cancellationToken: cancellationToken);
        var userIds = follows.Select(f => f.UserId).ToList();
        var users = await _unitOfWork.Repository<User>()
            .GetAllAsync(un => userIds.Contains(un.Id), cancellationToken: cancellationToken);

        foreach (var user in users)
        {
            var command = new SendMailCommand
            {
                To = user.Email.Value,
                Subject = "New Episode Created",
                Body = $"A new episode has been created in the series: {serie.Name}. Access it here: https://cukhoaito.id.vn/series/{serie.Id}"
            };
            await _publishEndpoint.Publish(command, cancellationToken);
        }

        await _sender.Send(new CreateNotificationCommand(
            "New Episode Created",
            $"A new episode has been created in the series: {serie.Name}.",
            userIds
        ), cancellationToken);
    }
}