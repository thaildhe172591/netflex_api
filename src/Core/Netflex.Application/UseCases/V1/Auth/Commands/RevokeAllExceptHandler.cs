using Netflex.Domain.Entities;

namespace Netflex.Application.UseCases.V1.Auth.Commands;

public record RevokeAllExceptCommand(string UserId, string? SessionId)
    : ICommand;

public class RevokeAllExceptHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    : ICommandHandler<RevokeAllExceptCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Unit> Handle(RevokeAllExceptCommand request, CancellationToken cancellationToken)
    {
        var sessionRepository = _unitOfWork.Repository<UserSession>();
        var sessions = await sessionRepository.GetAllAsync(x => x.UserId == request.UserId
            && !x.IsRevoked && x.Id != request.SessionId, cancellationToken) ?? [];

        foreach (var session in sessions) session.Revoke();

        await userRepository.ResetVersionByIdAsync(request.UserId, cancellationToken);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}