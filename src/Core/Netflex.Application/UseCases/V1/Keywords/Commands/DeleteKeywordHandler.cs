using FluentValidation;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Keywords.Commands;

public record DeleteKeywordCommand(long Id) : ICommand;

public class DeleteKeywordCommandValidator : AbstractValidator<DeleteKeywordCommand>
{
    public DeleteKeywordCommandValidator()
    {
        RuleFor(x => x.Id).NotNull();
    }
}

public class DeleteKeywordHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteKeywordCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteKeywordCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.Keyword>();

        var keyword = repository.Get(request.Id)
            ?? throw new NotFoundException(nameof(Domain.Entities.Keyword), request.Id);

        repository.Remove(keyword);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}