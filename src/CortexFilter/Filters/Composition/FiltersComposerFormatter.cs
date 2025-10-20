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


    public string GetJsonSchema() => $$"""
        {
          "definitions": {
            "logicalOperation": {
              "type": "object",
              "properties": {
                "type": {
                  "const": "logicalOperation"
                },
                "operation": {
                  "type": "string",
                  "enum": [
                    "or",
                    "and"
                  ]
                },
                "validations": {
                  "type": "array",
                  "oneOf": [
                    {
                      "$ref": "#/definitions/logicalOperation"
                    },
                    {
                      "$ref": "#/definitions/filter"
                    },
                    {
                      "$ref": "#/definitions/ambiguousFilter"
                    },
                    {
                      "$ref": "#/definitions/resource"
                    }
                  ]
                }
              },
              "required": [
                "type",
                "operation",
                "validations"
              ]
            },
            "filter": {
              "type": "object",
              "properties": {
                "type": {
                  "const": "filter"
                },
                "filterName": {
                  "type": "string",
                  "enum": [
                  {{string.Join(",\n", _concreteFilterFactories.Select(x => $"\t\"{x.Name}\""))}}
                  ]
                },
                "operation": {
                  "type": "string"
                },
                "value": {
                  "type": "string"
                }
              },
              "required": [
                "type",
                "filterName",
                "operation",
                "value"
              ]
            },
            "ambiguousFilter": {
              "properties": {
                "type": {
                  "const": "ambiguousFilter"
                },
                "filterName": {
                  "type": "string",
                  "enum": [
                  {{string.Join(",\n", _ambiguousFilters.Select(x => $"\t\"{x.Name}\""))}}
                  ]
                }
              },
              "required": [
                "type",
                "filterName"
              ]
            },
            "resource": {
              "properties": {
                "type": {
                  "const": "resource"
                },
                "resourceName": {
                  "type": "string",
                  "enum": [
                  {{string.Join(",\n", _resources.Select(x => $"\t\"{x.Name}\""))}}
                  ]
                }
              },
              "required": [
                "type",
                "resourceName"
              ]
            }
          },
          "type": "object",
          "properties": {
            "data":
            {
              "oneOf": [
                { "$ref": "#/definitions/logicalOperation" },
                { "$ref": "#/definitions/filter" },
                { "$ref": "#/definitions/ambiguousFilter" },
                { "$ref": "#/definitions/resource" }
              ]
            }
          },
          "required": [
            "data"
          ]
        }
        """;
}
