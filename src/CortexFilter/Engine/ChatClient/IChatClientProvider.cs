using OpenAI.Chat;

namespace CortexFilter.Engine;

/// <summary>
/// Interface used for registration of custom OpenAI client provider.
/// </summary>
public interface IChatClientProvider
{
    /// <summary>
    /// Provides client used for creation of filters.
    /// </summary>
    /// <returns>Instance of OpenAI chat client.</returns>
    ChatClient GetClient();
}
