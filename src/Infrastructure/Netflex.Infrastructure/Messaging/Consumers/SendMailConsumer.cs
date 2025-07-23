using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Netflex.Application.UseCases.V1.Users.Commands;

namespace Netflex.Infrastructure.Messaging.Consumers;

public class SendMailConsumer
    (ISender sender, ILogger<SendMailConsumer> logger)
    : IConsumer<SendMailCommand>
{
    public async Task Consume(ConsumeContext<SendMailCommand> context)
    {
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);
        await sender.Send(context.Message);
    }

}
