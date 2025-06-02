using Netflex.Application.Interfaces;
using Slugify;

namespace Netflex.Infrastructure.Services;

public class SlugGenerator : ISlugGenerator
{
    private readonly SlugHelper _slugHelper = new();
    public string GenerateSlug(string input)
    {
        return _slugHelper.GenerateSlug(input);
    }
}