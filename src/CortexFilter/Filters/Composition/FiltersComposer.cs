using CortexFilter.Filters.Composition;
using CortexFilter.Filters.Composition.LogicalOperations;
using CortexFilter.Filters.Composition.Responses;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CortexFilter.Filters;

internal class FiltersComposer<T>
{
    public FiltersComposerFormatter<T> Formatter { get; }
    private readonly IReadOnlyDictionary<string, IConcreteFilterFactory<T>> _concreteFilterFactories;
    public FiltersComposer(IEnumerable<IConcreteFilterFactory<T>> concreteFilterFactories)
    {
        Formatter = new FiltersComposerFormatter<T>(concreteFilterFactories);
        _concreteFilterFactories = concreteFilterFactories.ToDictionary(x => x.Name);
    }

    public ICollectionFilter<T>? Compose(string json)
    {
        try
        {
            var jObject = JsonObject.Parse(json);
            var root = jObject["data"];
            return ProcessNode(root);
        }
        catch { }
        return null;
    }
    private ICollectionFilter<T> ProcessNode(JsonNode node)
    {
        var type = node["type"].GetValue<string>();
        return type switch
        {
            "logicalOperation" => ProcessLogicalOperationNode(node),
            "filter" => ProcessFilterNode(node),
            _ => throw new NotSupportedException()
        };
    }
    private ICollectionFilter<T> ProcessLogicalOperationNode(JsonNode node)
    {
        var validationNodes = node["validations"].AsArray();
        var validations = validationNodes.Select(ProcessNode).ToArray();
        var operation = node["operation"].GetValue<string>();
        if (operation == LogicalOperationsConsts.OR_OPERATION)
            return new Or<T>(validations);
        if (operation == LogicalOperationsConsts.AND_OPERATION)
            return new And<T>(validations);
        throw new InvalidOperationException();
    }
    private IConcreteFilter<T> ProcessFilterNode(JsonNode node)
    {
        var filterResponse = JsonSerializer.Deserialize<FilterItemResponse>(node.ToJsonString());
        var factory = _concreteFilterFactories[filterResponse.Name];
        var filter = factory.CreateFilter(filterResponse.Operation, filterResponse.Value);
        return filter;
    }

}
