using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace CortexFilter.Filters.Composition.Schema;

internal class FilterDefinition<T>
{
    private readonly IEnumerable<IConcreteFilterFactory<T>> _filters;
    public FilterDefinition(IEnumerable<IConcreteFilterFactory<T>> filters)
    {
        _filters = filters;
    }

    public bool TryCreateSchema([NotNullWhen(true)] out JsonNode? node)
    {
        node = null;
        if (!_filters.Any())
            return false;

        var obj = new JsonObject();
        node = obj;
        obj.Add("type", JsonValue.Create("object"));
        var propertiesObj = new JsonObject
        {
            { "type", CreateTypeObject() },
            { "operation", CreateStringObject() },
            { "value", CreateStringObject() },
            { "filterName", CreateFiltersObject() }
        };
        obj.Add("properties", propertiesObj);
        obj.Add("required", new JsonArray(
            JsonValue.Create("type"),
            JsonValue.Create("operation"),
            JsonValue.Create("value"),
            JsonValue.Create("filterName")
        ));
        return true;
    }
    private static JsonObject CreateTypeObject()
    {
        var result = new JsonObject
        {
            { "const", JsonValue.Create("filter") }
        };
        return result;
    }
    private static JsonObject CreateStringObject()
    {
        var result = new JsonObject
        {
            { "type", JsonValue.Create("string") }
        };
        return result;
    }
    private JsonObject CreateFiltersObject()
    {
        var result = CreateStringObject();
        result.Add("enum", new JsonArray(_filters
            .Select(x => JsonValue.Create(x.Name))
            .ToArray()));
        return result;
    }

}
