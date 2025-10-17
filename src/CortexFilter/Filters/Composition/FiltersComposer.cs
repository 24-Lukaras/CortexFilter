using CortexFilter.Filters.Composition;
using CortexFilter.Filters.Composition.LogicalOperations;
using CortexFilter.Filters.Composition.Responses;
using OpenAI.Chat;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CortexFilter.Filters;

internal class FiltersComposer<T>
{
    private readonly List<IFilterInitializer<T>> _initializers = new List<IFilterInitializer<T>>();
    public FiltersComposerFormatter<T> Formatter { get; }
    private readonly IReadOnlyDictionary<string, IConcreteFilterFactory<T>> _concreteFilterFactories;
    private readonly IReadOnlyDictionary<string, AmbiguousFilter<T>> _ambiguousFilters;
    public FiltersComposer(IEnumerable<IConcreteFilterFactory<T>> concreteFilterFactories,
        IEnumerable<AmbiguousFilter<T>> ambiguousFilters)
    {
        Formatter = new FiltersComposerFormatter<T>(concreteFilterFactories, ambiguousFilters);
        _concreteFilterFactories = concreteFilterFactories.ToDictionary(x => x.Name);
        _ambiguousFilters = ambiguousFilters.ToDictionary(x => x.Name);
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
            "ambiguousFilter" => ProcessAmbiguousFilterNode(node),
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
    private ICollectionFilter<T> ProcessAmbiguousFilterNode(JsonNode node)
    {
        var response = JsonSerializer.Deserialize<AmbiguousFilterItemResponse>(node.ToJsonString());
        var filter = _ambiguousFilters[response.Name];
        _initializers.Add(filter);
        return filter;
    }

    public async Task RunInitializersAsync(string query, ChatClient client, IEnumerable<T> collection)
    {
        var tasks = _initializers
            .Select(x => x.InitAsync(query, client, collection))
            .ToArray();

        await Task.WhenAll(tasks);
        _initializers.Clear();
    }

}
