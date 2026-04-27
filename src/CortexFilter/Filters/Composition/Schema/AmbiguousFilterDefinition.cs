using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace CortexFilter.Filters.Composition.Schema;

internal class AmbiguousFilterDefinition<T>
{
    private readonly IEnumerable<AmbiguousFilter<T>> _filters;
    public AmbiguousFilterDefinition(IEnumerable<AmbiguousFilter<T>> filters)
    {
        _filters = filters;
    }

    public bool TryCreateSchema([NotNullWhen(true)] out JsonNode? node)
    {
        node = null;
        if (!_filters.Any())
            return false;
        var properties = new JsonObject()
        {
            { "type", new JsonObject()
                {
                    { "const", "ambiguousFilter" }
                }
            },
            { "filterName", new JsonObject()
                {
                    { "type", "string" },
                    { "enum", new JsonArray(_filters.Select(x => JsonValue.Create(x.Name)).ToArray()) },
                }
            }
        };
        var schema = new JsonObject()
        {
            { "properties", properties },
            { "required", new JsonArray("type", "filterName") }
        };
        node = schema;
        return true;
    }
}
