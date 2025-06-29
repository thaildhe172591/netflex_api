using System;

namespace Netflex.Infrastructure.Settings;

public record RefreshSettings
{
    public int ExpiresInDays { get; init; }
};
