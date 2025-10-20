using CortexFilter.Engine;
using OpenAI.Chat;

namespace CortexFilter.Filters;

internal interface IFilterInitializer<T>
{
    public Task InitAsync(FilterInitializerProperties<T> properties);
}

public record FilterInitializerProperties<T>(
    string Query,
    ChatClient Client,
    IEnumerable<T> Collection,
    ICortex Cortex
);
