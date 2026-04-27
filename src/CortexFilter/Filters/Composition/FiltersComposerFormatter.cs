using CortexFilter.Filters.Composition.Schema;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CortexFilter.Filters.Composition;

internal class FiltersComposerFormatter<T>
{
    private readonly IEnumerable<IConcreteFilterFactory<T>> _concreteFilterFactories;
    private readonly IEnumerable<AmbiguousFilter<T>> _ambiguousFilters;
    private readonly IEnumerable<ICortexResource<T>> _resources;
    public FiltersComposerFormatter(IEnumerable<IConcreteFilterFactory<T>> concreteFilterFactories,
        IEnumerable<AmbiguousFilter<T>> ambiguousFilters,
        IEnumerable<ICortexResource<T>> resources)
    {
        _concreteFilterFactories = concreteFilterFactories;
        _ambiguousFilters = ambiguousFilters;
        _resources = resources;
    }

    public string CreateMessageContent()
    {
        return $"""
            Here is a list of operations of and filters that can be used for filtering.{ConcreteFiltersInfo()}{AmbiguousFiltersInfo()}{ResourcesInfo()}
            You can use logical operations OR/AND to combine validations.
            Do not invent filters with filterName that is not listed.

            {FormatFilters()}
            {FormatAmbiguousFilters()}
            {FormatResources()}
            """;
    }
    private string ConcreteFiltersInfo() =>
        !_concreteFilterFactories.Any()
            ? string.Empty
            : "\nFilters are concrete validations that support certain operations agains a value.";
    private string AmbiguousFiltersInfo() =>
        !_ambiguousFilters.Any()
            ? string.Empty
            : "\nAmbiguous filters are ambiguous validations, that will be executed in further steps.";
    private string ResourcesInfo() =>
        !_resources.Any()
            ? string.Empty
            : "\nResources are filters, that depend on other entities in the system.";
    private string FormatFilters() =>
        !_concreteFilterFactories.Any()
            ? string.Empty
            : $$"""
                Filters will be listed in following format (description is optional):
                {FilterName} - [{ListOfSupportedOperations}]
                    "{FilterDescription}"

                Operations:
                    eq - Equals
                    gt - Greater than
                    ge - Greater or equal
                    lt - Lesser than
                    le - Lesser or equal
                    contains - String contains a value
                    startsWith - String starts with value
                    endsWith - String ends with value

                Filters:
                {{FormatFilterFactories()}}

                """;
    private string FormatFilterFactories() =>
        string.Join("\n", _concreteFilterFactories.Select(FormatFilterFactory));
    private string FormatFilterFactory(IConcreteFilterFactory<T> factory)
    {
        if (string.IsNullOrEmpty(factory.Description))
            return $"\t{factory.Name} - [{string.Join(", ", factory.SupportedOperations)}]";
        return $"\t{factory.Name} - [{string.Join(", ", factory.SupportedOperations)}]\n\t\t\"{factory.Description}\"";
    }
    private string FormatAmbiguousFilters() =>
        !_ambiguousFilters.Any()
            ? string.Empty
            : $$"""
                Ambiguous filters will be listed in following format (description is optional):
                    {FilterName} - "{FilterDescription}"

                Ambiguous filters:
                {{FormatAmbiguousFilterInstances()}}

                """;
    private string FormatAmbiguousFilterInstances() =>
        string.Join("\n", _ambiguousFilters.Select(FormatAmbiguousFilter));
    private string FormatAmbiguousFilter(AmbiguousFilter<T> filter)
    {
        if (string.IsNullOrEmpty(filter.Description))
            return $"\t{filter.Name}";
        return $"\t{filter.Name} - \"{filter.Description}\"";
    }
    private string FormatResources() =>
        !_resources.Any()
            ? string.Empty
            : $$"""
                Resources will be listed in following format (description is optional):
                    {ResourceName} - "{ResourceDescription}"

                Resources:
                {{FormatResourceInstances()}}

                """;
    private string FormatResourceInstances() =>
        string.Join("\n", _resources.Select(FormatResource));
    private string FormatResource(ICortexResource<T> filter)
    {
        if (string.IsNullOrEmpty(filter.Description))
            return $"\t{filter.Name}";
        return $"\t{filter.Name} - \"{filter.Description}\"";
    }

    public string GetJsonSchema()
    {
        List<string> references = new List<string>()
        {
            "#/definitions/logicalOperation"
        };
        JsonObject definitions = new JsonObject();
        var filterDef = new FilterDefinition<T>(_concreteFilterFactories);
        if (filterDef.TryCreateSchema(out var filterDefSchema))
        {
            references.Add("#/definitions/filter");
            definitions.Add("filter", filterDefSchema);
        }
        var amFilterDef = new AmbiguousFilterDefinition<T>(_ambiguousFilters);
        if (amFilterDef.TryCreateSchema(out var amFilterDefSchema))
        {
            references.Add("#/definitions/ambiguousFilter");
            definitions.Add("ambiguousFilter", amFilterDefSchema);
        }
        var resourcesDef = new ResourceFilterDefinition<T>(_resources);
        if (resourcesDef.TryCreateSchema(out var resourcesDefSchema))
        {
            references.Add("#/definitions/resource");
            definitions.Add("resource", resourcesDefSchema);
        }
        var logicalOperationDef = new LogicalOperationDefinition(references);
        definitions.Add("logicalOperation", logicalOperationDef.CreateSchema());

        JsonObject result = new JsonObject()
        {
            { "type", "object" },
            { "definitions", definitions },
            { "properties", new JsonObject()
                {
                    { "data", new JsonObject()
                        {
                            { "oneOf", new JsonArray(references.Select(x =>
                                new JsonObject()
                                {
                                    { "$ref", x }
                                }).ToArray())
                            }
                        }
                    }
                }
            },
            { "required", new JsonArray("data") }
        };
        return result.ToJsonString();
    }
}
