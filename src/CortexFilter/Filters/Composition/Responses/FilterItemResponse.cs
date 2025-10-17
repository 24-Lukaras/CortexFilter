using System.Text.Json.Serialization;

namespace CortexFilter.Filters.Composition.Responses;

internal class FilterItemResponse
{
    [JsonPropertyName("filterName")]
    public string Name { get; init; } = null!;

    [JsonPropertyName("operation")]
    public string Operation { get; init; } = null!;

    [JsonPropertyName("value")]
    public string Value { get; init; } = null!;
}
