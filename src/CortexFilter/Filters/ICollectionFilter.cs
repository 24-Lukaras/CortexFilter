namespace CortexFilter.Filters;

public interface ICollectionFilter<T>
{
    public IEnumerable<T> Filter(IEnumerable<T> collection);
}
