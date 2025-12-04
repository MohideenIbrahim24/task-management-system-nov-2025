using Application.Interfaces; 
using Microsoft.Extensions.Caching.Memory;
namespace Infrastructure.Services;
public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _mem;
    public MemoryCacheService(IMemoryCache mem) { _mem = mem; }
    public bool TryGetValue<T>(string key, out T? value) => _mem.TryGetValue(key, out value);
    public void Set<T>(string key, T value, TimeSpan ttl) => _mem.Set(key, value, ttl);
    public void Remove(string key) => _mem.Remove(key);
}
