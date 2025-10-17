using CortexFilter.Filters;
using OpenAI.Chat;

namespace CortexFilter;

public abstract class NaturalLanguageEngine<T> : INaturalLanguageEngine<T>
{
    private readonly INaturalLanguageEngineProperties<T> _properties;
    public NaturalLanguageEngine(INaturalLanguageEngineProperties<T> properties)
    {
        _properties = properties;
    }

    public abstract Task<IEnumerable<T>> GetDataAsync();
    public abstract string? ContextSystemMessage { get; }

    public async Task<IEnumerable<T>> SearchAsync(string query)
    {
        var filterComposer = new FiltersComposer<T>(_properties.ConcreteFilterFactories,
            _properties.AmbiguousFilters);

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
        await filterComposer.RunInitializersAsync(query, client, data);
        var filteredData = filter.Filter(data);
        return filteredData;
    }
}
