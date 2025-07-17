using FluentValidation;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;
using Unit = MediatR.Unit;

namespace Netflex.Application.UseCases.V1.Reports.Commands;

public record UpdateReportCommand(long Id, string? Reason, string? Description) : ICommand;
public class UpdateReportCommandValidator : AbstractValidator<UpdateReportCommand>
{
    public UpdateReportCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
public class UpdateReportHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateReportCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Report>();

        var report = await repository.GetAsync(r => r.Id == request.Id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Report), request.Id);

        report.Update(request.Reason, request.Description);

        repository.Update(report);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}