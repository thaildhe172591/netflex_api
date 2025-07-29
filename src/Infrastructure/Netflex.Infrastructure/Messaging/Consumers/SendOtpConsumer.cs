using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Netflex.Application.UseCases.V1.Auth.Commands;

namespace Netflex.Infrastructure.Messaging.Consumers;

public class SendOtpConsumer
    (ISender sender, ILogger<SendOtpConsumer> logger)
    : IConsumer<SendOtpCommand>
{
    public async Task Consume(ConsumeContext<SendOtpCommand> context)
    {
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);
        await sender.Send(context.Message);
    }
}