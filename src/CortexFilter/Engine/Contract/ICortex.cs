namespace CortexFilter.Engine;

public interface ICortex
{
    public Task<IEnumerable<T>> Search<T>(string query);
}
