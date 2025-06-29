using Microsoft.AspNetCore.Http;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using Netflex.Application.Exceptions;
using Netflex.Application.Interfaces;

namespace Netflex.Infrastructure.Services;

public class CloudStorage : ICloudStorage
{
    private readonly StorageClient _client;
    private readonly GoogleCredential _credential;
    private readonly string _bucketName;
    public CloudStorage(IOptions<GoogleSettings> options)
    {
        if (options.Value is null) throw new NotConfiguredException(nameof(GoogleSettings));

        _credential = GoogleCredential.FromFile(options.Value.CredentialsFile);
        _client = StorageClient.Create(_credential);
        _bucketName = options.Value.BucketName;
    }
    public async Task<Uri> UploadAsync(string name, IFileResource file)
    {
        var id = Guid.NewGuid();
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        var blob = await _client.UploadObjectAsync(_bucketName,
            $"{name}-{id}", file.ContentType, stream);

        return new Uri(blob.MediaLink);
    }
}
