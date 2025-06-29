namespace Netflex.Application.Interfaces;

public interface IFileResource
{
    string Name { get; }
    string FileName { get; }
    string ContentType { get; }
    long Length { get; }
    void CopyTo(Stream target);
    Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
    Stream OpenReadStream();
}