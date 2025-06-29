using FluentValidation;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;
namespace Netflex.Application.UseCases.V1.Episodes.Commands;

public record DeleteEpisodeCommand(long Id) : ICommand;

public class DeleteEpisodeCommandValidator : AbstractValidator<DeleteEpisodeCommand>
{
    public DeleteEpisodeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteEpisodeHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteEpisodeCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteEpisodeCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Episode>();

        var episode = repository.Get(request.Id)
            ?? throw new NotFoundException(nameof(Episode), request.Id);

        repository.Remove(episode);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}