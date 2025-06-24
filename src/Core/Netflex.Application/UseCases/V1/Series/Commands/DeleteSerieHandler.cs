using FluentValidation;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Series.Commands;

public record DeleteSerieCommand(long Id) : ICommand;
public class DeleteSerieCommandValidator : AbstractValidator<DeleteSerieCommand>
{
    public DeleteSerieCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteSerieHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteSerieCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteSerieCommand request, CancellationToken cancellationToken)
    {
        var serieRepository = _unitOfWork.Repository<TVSerie>();

        var serie = await serieRepository
            .GetAsync(m => m.Id == request.Id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(TVSerie), request.Id);

        serieRepository.Remove(serie);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}