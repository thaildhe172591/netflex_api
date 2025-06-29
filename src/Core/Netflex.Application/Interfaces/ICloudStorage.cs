namespace Netflex.Application.Interfaces;

public interface ICloudStorage
{
    Task<Uri> UploadAsync(string name, IFileResource file);
}
