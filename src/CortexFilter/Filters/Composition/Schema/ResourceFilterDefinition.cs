using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace CortexFilter.Filters.Composition.Schema;

internal class ResourceFilterDefinition<T>
{
    private readonly IEnumerable<ICortexResource<T>> _resources;
    public ResourceFilterDefinition(IEnumerable<ICortexResource<T>> resources)
    {
        _resources = resources;
    }

    public bool TryCreateSchema([NotNullWhen(true)] out JsonNode? node)
    {
        node = null;
        if (!_resources.Any())
            return false;
        var properties = new JsonObject()
        {
            { "type", new JsonObject()
                {
                    { "const", "resource" }
                }
            },
            { "resourceName", new JsonObject()
                {
                    { "type", "string" },
                    { "enum", new JsonArray(_resources.Select(x => JsonValue.Create(x.Name)).ToArray()) },
                }
            }
        };
        var schema = new JsonObject()
        {
            { "properties", properties },
            { "required", new JsonArray("type", "resourceName") }
        };
        node = schema;
        return true;
    }
}
