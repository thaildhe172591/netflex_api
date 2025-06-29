using Microsoft.AspNetCore.Http;
using Netflex.Application.Interfaces;

namespace Netflex.Infrastructure.Services;

public class FormFileAdapter : IFileResource
{
    private readonly IFormFile _formFile;

    public FormFileAdapter(IFormFile formFile)
    {
        _formFile = formFile;
    }
    public static IFileResource? From(IFormFile? formFile)
    {
        return formFile is null ? null : new FormFileAdapter(formFile);
    }


    public string Name => _formFile.Name;
    public string FileName => _formFile.FileName;
    public string ContentType => _formFile.ContentType;
    public long Length => _formFile.Length;
    public void CopyTo(Stream target)
    {
        _formFile.CopyTo(target);
    }
    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
    {
        return _formFile.CopyToAsync(target, cancellationToken);
    }
    public Stream OpenReadStream()
    {
        return _formFile.OpenReadStream();
    }
}