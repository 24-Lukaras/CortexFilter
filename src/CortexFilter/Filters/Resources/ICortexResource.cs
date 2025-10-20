namespace CortexFilter.Filters;

internal interface ICortexResource<T> : IFilterInitializer<T>, ICollectionFilter<T>
{
    public string Name { get; }
    public string? Description { get; }
}
