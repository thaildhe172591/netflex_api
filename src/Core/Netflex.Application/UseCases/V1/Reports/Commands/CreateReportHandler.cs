using FluentValidation;
using Netflex.Domain.Entities;
using Netflex.Domain.Enumerations;

namespace Netflex.Application.UseCases.V1.Reports.Commands;

public record CreateReportCommand(string Reason, string? Description) : ICommand<CreateReportResult>;
public record CreateReportResult(long Id);

public class CreateReportCommandValidator : AbstractValidator<CreateReportCommand>
{
    public CreateReportCommandValidator()
    {
        RuleFor(x => x.Reason).NotEmpty();
    }
}

public class CreateReportHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<CreateReportCommand, CreateReportResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateReportResult> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var report = Report.Create(request.Reason, request.Description, Process.Open);
        await _unitOfWork.Repository<Report>().AddAsync(report, cancellationToken);
        await _unitOfWork.CommitAsync();
        return new CreateReportResult(report.Id);
    }
}