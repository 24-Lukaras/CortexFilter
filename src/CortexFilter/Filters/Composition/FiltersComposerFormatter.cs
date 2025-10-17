namespace CortexFilter.Filters.Composition;

internal class FiltersComposerFormatter<T>
{
    private readonly IEnumerable<IConcreteFilterFactory<T>> _concreteFilterFactories;
    public FiltersComposerFormatter(IEnumerable<IConcreteFilterFactory<T>> concreteFilterFactories)
    {
        _concreteFilterFactories = concreteFilterFactories;
    }

    public string CreateMessageContent()
    {
        return $"""
            Here is a list of operations of and filters that can be used for filtering.{ConcreteFiltersInfo()}
            You can use logical operations OR/AND to combine validations.

            {FormatFilters()}
            """;
    }
    private string ConcreteFiltersInfo() =>
        !_concreteFilterFactories.Any()
            ? string.Empty
            : "\nFilters are concrete validations that support certain operations agains a value.";
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
            }
          },
          "type": "object",
          "properties": {
            "data":
            {
              "oneOf": [
                { "$ref": "#/definitions/logicalOperation" },
                { "$ref": "#/definitions/filter" }
              ]
            }
          },
          "required": [
            "data"
          ]
        }
        """;
}
