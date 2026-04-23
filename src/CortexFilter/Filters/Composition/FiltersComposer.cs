using CortexFilter.Filters.Composition;
using CortexFilter.Filters.Composition.LogicalOperations;
using CortexFilter.Filters.Composition.Responses;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CortexFilter.Filters;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
internal class FiltersComposer<T>
{
    private readonly List<IFilterInitializer<T>> _initializers = new List<IFilterInitializer<T>>();
    public FiltersComposerFormatter<T> Formatter { get; }
    private readonly IReadOnlyDictionary<string, IConcreteFilterFactory<T>> _concreteFilterFactories;
    private readonly IReadOnlyDictionary<string, AmbiguousFilter<T>> _ambiguousFilters;
    private readonly IReadOnlyDictionary<string, ICortexResource<T>> _resources;
    public FiltersComposer(IEnumerable<IConcreteFilterFactory<T>> concreteFilterFactories,
        IEnumerable<AmbiguousFilter<T>> ambiguousFilters,
        IEnumerable<ICortexResource<T>> resources)
    {
        _concreteFilterFactories = concreteFilterFactories.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.First());
        _ambiguousFilters = ambiguousFilters.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.First());
        _resources = resources.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.First());
        Formatter = new FiltersComposerFormatter<T>(_concreteFilterFactories.Values, _ambiguousFilters.Values, _resources.Values);
    }

    /// <summary>
    /// Composes a filter based on provided JSON.
    /// </summary>
    /// <param name="json">JSON definition of filter.</param>
    /// <returns>Created filter or null on error.</returns>
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

    /// <summary>
    /// Processes a JsonNode and returns filter based on its definition.
    /// </summary>
    /// <param name="node"><see cref="JsonNode"/> containing filter definition.</param>
    /// <returns>Created filter.</returns>
    /// <exception cref="NotSupportedException"></exception>
    private ICollectionFilter<T> ProcessNode(JsonNode node)
    {
        var type = node["type"].GetValue<string>();
        return type switch
        {
            "logicalOperation" => ProcessLogicalOperationNode(node),
            "filter" => ProcessFilterNode(node),
            "ambiguousFilter" => ProcessAmbiguousFilterNode(node),
            "resource" => ProcessResourceNode(node),
            _ => throw new NotSupportedException()
        };
    }

    /// <summary>
    /// Creates instance of either <see cref="And{T}"/> or <see cref="Or{T}"/>.
    /// </summary>
    /// <param name="node"><see cref="JsonNode"/> containing filter definition.</param>
    /// <returns>Composite filter.</returns>
    /// <exception cref="InvalidOperationException"></exception>
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

    /// <summary>
    /// Creates <see cref="IConcreteFilter{T}{T}"/>> filter.
    /// </summary>
    /// <param name="node"><see cref="JsonNode"/> containing filter definition.</param>
    /// <returns>Created concrete filter.</returns>
    private IConcreteFilter<T> ProcessFilterNode(JsonNode node)
    {
        var filterResponse = JsonSerializer.Deserialize<FilterItemResponse>(node.ToJsonString());
        var factory = _concreteFilterFactories[filterResponse.Name];
        var filter = factory.CreateFilter(filterResponse.Operation, filterResponse.Value);
        return filter;
    }

    /// <summary>
    /// Creates <see cref="AmbiguousFilter{T}"/>> filter.
    /// </summary>
    /// <param name="node"><see cref="JsonNode"/> containing filter definition.</param>
    /// <returns>Created ambiguous filter.</returns>
    private ICollectionFilter<T> ProcessAmbiguousFilterNode(JsonNode node)
    {
        var response = JsonSerializer.Deserialize<AmbiguousFilterItemResponse>(node.ToJsonString());
        var filter = _ambiguousFilters[response.Name];
        _initializers.Add(filter);
        return filter;
    }

    /// <summary>
    /// Creates <see cref="CortexResource{TSource, TResult}"/> filter.
    /// </summary>
    /// <param name="node"><see cref="JsonNode"/> containing resource definition.</param>
    /// <returns>Created resource filter.</returns>
    private ICollectionFilter<T> ProcessResourceNode(JsonNode node)
    {
        var response = JsonSerializer.Deserialize<ResourceItemResponse>(node.ToJsonString());
        var filter = _resources[response.Name];
        _initializers.Add(filter);
        return filter;
    }

    /// <summary>
    /// Initializes all instances of <see cref="IFilterInitializer{T}"/>.
    /// </summary>
    /// <param name="properties">Bundled properties required by <see cref="IFilterInitializer{T}.InitAsync(FilterInitializerProperties{T})"/>.</param>
    /// <returns>Asynchronous task.</returns>
    public async Task RunInitializersAsync(FilterInitializerProperties<T> properties)
    {
        var tasks = _initializers
            .Select(x => x.InitAsync(properties))
            .ToArray();

        await Task.WhenAll(tasks);
        _initializers.Clear();
    }

}
