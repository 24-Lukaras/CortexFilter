using System.Text.Json.Serialization;

namespace CortexFilter.Filters.Composition.Responses;

internal class ResourceItemResponse
{
    [JsonPropertyName("resourceName")]
    public string Name { get; init; } = null!;
}