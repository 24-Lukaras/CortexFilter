namespace CortexFilter.Filters;

public interface IConcreteFilterFactory<T>
{
    string Name { get; }
    string? Description { get; }
    string[] SupportedOperations { get; }

    IConcreteFilter<T> CreateFilter(string operation, string value);
}
