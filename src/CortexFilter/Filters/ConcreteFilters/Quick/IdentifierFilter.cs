using OpenAI.Chat;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CortexFilter.Filters;

public abstract class IdentifierFilter<T, TItem> : AmbiguousFilter<T>
{
    protected abstract Task<IEnumerable<TItem>> GetItemsAsync();
    protected abstract (string Id, string Title) FormatItem(TItem item);
    protected HashSet<string> _ids = new HashSet<string>();
    protected override async Task InitInternalAsync(string query, ChatClient client, IEnumerable<T> collection)
    {
        var items = await GetItemsAsync();
        var itemsDic = items.ToDictionary(x => FormatItem(x).Id);
        string message = $"""
            User has entered following query:
            "{query}"

            I will provide list of items in following format: Id|Title
            Please return ids of items that satisfy the query.

            Here is the list:
            {string.Join('\n', items.Select(x => { var parts = FormatItem(x); return $"{parts.Id}|{parts.Title}"; }))}
            """;
        var chatMessage = ChatMessage.CreateUserMessage(message);
        var response = await client.CompleteChatAsync([message], new ChatCompletionOptions()
        {
            ResponseFormat = JSON_FORMAT
        });
        var content = response.Value.Content[0].Text;
        var result = JsonSerializer.Deserialize<IdentifierFilterResponse>(content);
        _ids = result.Ids.ToHashSet();
    }

    private static readonly ChatResponseFormat JSON_FORMAT = ChatResponseFormat.CreateJsonSchemaFormat("identifier_response",
        BinaryData.FromString("""
        {
            "type": "object",
            "properties": {
                "ids": {
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                }
            },
            "required": []
        }
        """));
}
internal class IdentifierFilterResponse
{
    [JsonPropertyName("ids")]
    public string[] Ids { get; init; }
}
