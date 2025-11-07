using CortexFilter.Filters;
using OpenAI.Chat;

namespace CortexFilter;

/// <summary>
/// Abstract class for natural language filtering. Needs <see cref="GetDataAsync"/> and <see cref="ContextSystemMessage"/> implemented.
/// </summary>
/// <typeparam name="T">Type of filtered data.</typeparam>
public abstract class NaturalLanguageEngine<T> : INaturalLanguageEngine<T>
{
    private readonly INaturalLanguageEngineProperties<T> _properties;
    public NaturalLanguageEngine(INaturalLanguageEngineProperties<T> properties)
    {
        _properties = properties;
    }

    /// <summary>
    /// Async function that retrieves data which are used for filtering.
    /// </summary>
    /// <returns>Collection of data to be filtered.</returns>
    public abstract Task<IEnumerable<T>> GetDataAsync();

    /// <summary>
    /// Defines additional context that is sent to LLM.
    /// </summary>
    public abstract string? ContextSystemMessage { get; }

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> SearchAsync(string query)
    {
        var filterComposer = new FiltersComposer<T>(_properties.ConcreteFilterFactories,
            _properties.AmbiguousFilters,
            _properties.Resources);

        var client = _properties.ClientProvider.GetClient();

        List<ChatMessage> messages = new List<ChatMessage>();
        if (!string.IsNullOrEmpty(ContextSystemMessage))
        {
            messages.Add(ChatMessage.CreateSystemMessage(ContextSystemMessage));
        }
        messages.Add(ChatMessage.CreateSystemMessage(filterComposer.Formatter.CreateMessageContent()));
        messages.Add(ChatMessage.CreateUserMessage($"""
            Please provide list of filters to use for following query:
            "{query}"
            """));

        var response = await client.CompleteChatAsync(messages.ToArray(), new ChatCompletionOptions()
        {
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat("response_format", BinaryData.FromString(filterComposer.Formatter.GetJsonSchema()))
        });

        var content = response.Value.Content[0].Text;
        var filter = filterComposer.Compose(content);

        if (filter is null)
            return Array.Empty<T>();

        var data = await GetDataAsync();
        var initializerProperties = new FilterInitializerProperties<T>(query, client, data, _properties.Cortex);
        await filterComposer.RunInitializersAsync(initializerProperties);
        var filteredData = filter.Filter(data);
        return filteredData;
    }
}
