namespace CortexFilter;

public interface INaturalLanguageEngine<T>
{
    public Task<IEnumerable<T>> SearchAsync(string query);
}
