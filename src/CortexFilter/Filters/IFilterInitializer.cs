using OpenAI.Chat;

namespace CortexFilter.Filters;

internal interface IFilterInitializer<T>
{
    Task InitAsync(string query, ChatClient client, IEnumerable<T> collection);
}
