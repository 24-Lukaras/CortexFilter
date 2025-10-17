using System.Text.Json.Serialization;

namespace CortexFilter.Filters.Composition.Responses;

internal class AmbiguousFilterItemResponse
{
    [JsonPropertyName("filterName")]
    public string Name { get; init; } = null!;
}