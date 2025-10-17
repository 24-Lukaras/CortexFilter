using CortexFilter.Filters.Composition.LogicalOperations;
using CortexFilter.Filters.Composition.Responses;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CortexFilter.Filters;

internal class FiltersComposer<T>
{
    private readonly IReadOnlyDictionary<string, IConcreteFilterFactory<T>> _concreteFilterFactories;
    public FiltersComposer(IEnumerable<IConcreteFilterFactory<T>> concreteFilterFactories)
    {
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
        string.Join("\n", _concreteFilterFactories.Values.Select(FormatFilterFactory));
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
                  {{string.Join(",\n", _concreteFilterFactories.Values.Select(x => $"\t\"{x.Name}\""))}}
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
