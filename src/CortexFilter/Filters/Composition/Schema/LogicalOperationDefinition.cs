using System.Text.Json.Nodes;

namespace CortexFilter.Filters.Composition.Schema;

internal class LogicalOperationDefinition
{
    private readonly JsonObject[] _references;
    public LogicalOperationDefinition(IEnumerable<string> references)
    {
        _references = references
            .Select(x => new JsonObject()
            {
                { "$ref", x }
            }).ToArray();
    }
    public JsonNode CreateSchema()
    {
        var properties = new JsonObject()
        {
            { "type", new JsonObject()
                {
                    { "const", "logicalOperation" }
                }
            },
            {
                "operation", new JsonObject()
                {
                    { "type", "string" },
                    { "enum", new JsonArray(
                        "or",
                        "and")
                    }
                }
            },
            {
                "validations", new JsonObject()
                {
                    { "type", "array" },
                    { "oneOf", new JsonArray(_references) }
                }
            }
        };
        var node = new JsonObject
        {
            { "type", "object" },
            { "properties", properties },
            { "required", new JsonArray(
                "type",
                "operation",
                "validations"
            )}
        };
        return node;
    }
}
