using OpenAI.Chat;

namespace CortexFilter.Engine;

public interface IChatClientProvider
{
    ChatClient GetClient();
}
