using FluentValidation;

namespace Netflex.Application.Validation;

public static class FileResourceValidationExtensions
{
    public static IRuleBuilderOptions<T, IFileResource?> MaxFileSize<T>(
        this IRuleBuilder<T, IFileResource?> ruleBuilder, long maxMB)
    {
        var maxBytes = maxMB * 1024 * 1024;
        return ruleBuilder.Must(file => file == null || file.Length <= maxBytes)
            .WithMessage($"File size must be less than or equal to {maxMB} MB");
    }

    public static IRuleBuilderOptions<T, IFileResource?> AllowedExtensions<T>(
        this IRuleBuilder<T, IFileResource?> ruleBuilder, params string[] extensions)
    {
        return ruleBuilder.Must(file =>
        {
            if (file == null) return true;

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            return !string.IsNullOrEmpty(ext) && extensions.Contains(ext);
        })
        .WithMessage($"File must have one of the following extensions: {string.Join(", ", extensions)}");
    }
}
