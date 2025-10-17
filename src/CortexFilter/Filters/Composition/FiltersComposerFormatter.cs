namespace CortexFilter.Filters.Composition;

internal class FiltersComposerFormatter<T>
{
    private readonly IEnumerable<IConcreteFilterFactory<T>> _concreteFilterFactories;
    private readonly IEnumerable<AmbiguousFilter<T>> _ambiguousFilters;
    public FiltersComposerFormatter(IEnumerable<IConcreteFilterFactory<T>> concreteFilterFactories,
        IEnumerable<AmbiguousFilter<T>> ambiguousFilters)
    {
        _concreteFilterFactories = concreteFilterFactories;
        _ambiguousFilters = ambiguousFilters;
    }

    public string CreateMessageContent()
    {
        return $"""
            Here is a list of operations of and filters that can be used for filtering.{ConcreteFiltersInfo()}{AmbiguousFiltersInfo()}
            You can use logical operations OR/AND to combine validations.

            {FormatFilters()}
            {FormatAmbiguousFilters()}
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
            }
          },
          "type": "object",
          "properties": {
            "data":
            {
              "oneOf": [
                { "$ref": "#/definitions/logicalOperation" },
                { "$ref": "#/definitions/filter" },
                { "$ref": "#/definitions/ambiguousFilter" }
              ]
            }
          },
          "required": [
            "data"
          ]
        }
        """;
}
