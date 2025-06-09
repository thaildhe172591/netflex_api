using Microsoft.Extensions.Configuration;
using Netflex.Application.Exceptions;
using Netflex.Application.Interfaces;
using Google.Apis.Auth.OAuth2.Flows;
using Netflex.Domain.ValueObjects;


namespace Netflex.Infrastructure.Services;

public record GoogleSettings(string ClientId, string ClientSecret, string UserInfoUrl);
internal record UserInfoResponse(string Sub, string Email, string Name, string Picture)
{
    public UserInfo ToUserInfo() => new(Sub, Name, Email, Picture);
};

public class GoogleLoginService(IConfiguration configuration, IHttpClientFactory factory)
    : ISocialLoginService
{
    private readonly IHttpClientFactory _factory = factory;
    private readonly GoogleSettings _settings = configuration.GetSection(nameof(GoogleSettings)).Get<GoogleSettings>()
        ?? throw new NotConfiguredException(nameof(GoogleSettings));
    public LoginProvider Provider => LoginProvider.Google;
    public async Task<UserInfo> FetchUserInfoAsync(string code, string redirect)
    {
        var flow = new GoogleAuthorizationCodeFlow(new()
        {
            ClientSecrets = new()
            {
                ClientId = _settings.ClientId,
                ClientSecret = _settings.ClientSecret
            }
        });

        var tokenResponse = await flow.ExchangeCodeForTokenAsync(
            userId: "me",
            code: code,
            redirectUri: redirect,
            taskCancellationToken: CancellationToken.None);

        using var httpClient = _factory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", tokenResponse.AccessToken);

        var response = await httpClient.GetStringAsync(_settings.UserInfoUrl);

        return Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoResponse>(response)?.ToUserInfo()
            ?? throw new InvalidOperationException();
    }

    public async Task<string> GetLoginUrlAsync(string redirectUrl)
    {
        await Task.CompletedTask;

        var scope = "openid email profile";
        return $"https://accounts.google.com/o/oauth2/v2/auth?response_type=code" +
            $"&client_id={_settings.ClientId}" +
            $"&redirect_uri={Uri.EscapeDataString(redirectUrl)}" +
            $"&scope={Uri.EscapeDataString(scope)}";
    }
}