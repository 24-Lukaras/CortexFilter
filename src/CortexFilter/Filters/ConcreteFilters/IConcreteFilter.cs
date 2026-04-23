namespace CortexFilter.Filters;

/// <summary>
/// Interface for concrete filters created by <see cref="IConcreteFilterFactory{T}"/>.
/// </summary>
/// <typeparam name="T">Type of filtered data.</typeparam>
public interface IConcreteFilter<T> : ICollectionFilter<T>
{

}
