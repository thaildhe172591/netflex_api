using Netflex.Domain.Entities;

namespace Netflex.Application.UseCases.V1.Auth.Commands;

public record RevokeSessionsCommand(string UserId, string? ExceptId = default)
    : ICommand;

public class RevokeSessionsHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    : ICommandHandler<RevokeSessionsCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserRepository _userRepository = userRepository;
    public async Task<Unit> Handle(RevokeSessionsCommand request, CancellationToken cancellationToken)
    {
        var sessionRepository = _unitOfWork.Repository<UserSession>();
        var sessions = await sessionRepository.GetAllAsync(x => x.UserId == request.UserId
            && !x.IsRevoked && x.Id != request.ExceptId, cancellationToken: cancellationToken) ?? [];

        foreach (var session in sessions) session.Revoke();

        await _userRepository.ResetVersionByIdAsync(request.UserId, cancellationToken);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}