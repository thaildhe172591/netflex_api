using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Reports.Commands;

public record DeleteReportCommand(long Id) : ICommand;
public class DeleteReportHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteReportCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Report>();
        var report = repository.Get(request.Id)
            ?? throw new NotFoundException(nameof(Report), request.Id);

        repository.Remove(report);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}