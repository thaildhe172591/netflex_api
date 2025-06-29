using FluentValidation;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;
namespace Netflex.Application.UseCases.V1.Actors.Commands;

public record DeleteActorCommand(long Id) : ICommand;

public class DeleteActorCommandValidator : AbstractValidator<DeleteActorCommand>
{
    public DeleteActorCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteActorHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteActorCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteActorCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Actor>();

        var actor = repository.Get(request.Id)
            ?? throw new NotFoundException(nameof(Actor), request.Id);

        repository.Remove(actor);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}