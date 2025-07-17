using Netflex.Domain.Entities;
using Netflex.Domain.ValueObjects;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Users.Commands;

public record FollowCommand(string UserId, string TargetId, string TargetType) : ICommand;

public class FollowHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<FollowCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Unit> Handle(FollowCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>()
            .GetAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken)
            ?? throw new UserNotFoundException();

        var target = TargetType.Of(request.TargetType);
        if (target == TargetType.Movie && int.TryParse(request.TargetId, out var movieId))
        {
            var isExist = await _unitOfWork.Repository<Movie>()
                .ExistsAsync(m => m.Id == movieId, cancellationToken: cancellationToken);
            if (!isExist) throw new NotFoundException(nameof(Movie), movieId);
        }
        else if (target == TargetType.TVSerie && int.TryParse(request.TargetId, out var serieId))
        {
            var isExist = await _unitOfWork.Repository<TVSerie>()
                .ExistsAsync(m => m.Id == serieId, cancellationToken: cancellationToken);
            if (!isExist) throw new NotFoundException(nameof(TVSerie), serieId);
        }
        else throw new NotFoundException(request.TargetType, request.TargetId);


        var followRepository = _unitOfWork.Repository<Follow>();

        if (await followRepository.ExistsAsync(f => f.UserId == request.UserId
            && f.TargetId == request.TargetId
            && f.TargetType == target, cancellationToken: cancellationToken))
            return Unit.Value;

        try
        {
            var follow = Follow.Create(request.TargetId, target, request.UserId, DateTime.UtcNow);
            await followRepository.AddAsync(follow, cancellationToken);
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}