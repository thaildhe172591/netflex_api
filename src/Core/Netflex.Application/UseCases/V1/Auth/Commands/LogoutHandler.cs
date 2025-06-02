using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Auth.Commands;

public record LogoutCommand(string AccessJti, string UserId, string DeviceId)
    : ICommand;

public class LogoutHandler(IUnitOfWork unitOfWork, IJwtTokenService tokenService) : ICommandHandler<LogoutCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IJwtTokenService _tokenService = tokenService;
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        //STEP 1: Get current session 
        var userSession = await _unitOfWork.Repository<UserSession>()
            .GetAsync(x => x.UserId == request.UserId && x.DeviceId == request.DeviceId && !x.IsRevoked, cancellationToken)
            ?? throw new NotFoundException(nameof(UserSession), new { request.UserId, request.DeviceId });

        //STEP 2: Revoke refresh token and access token
        userSession.Revoke();

        await _tokenService.AddToBlacklistAsync(request.AccessJti);

        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}