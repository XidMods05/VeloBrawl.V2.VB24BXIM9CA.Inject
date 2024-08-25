using System.Collections.Concurrent;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.AbstractionLayer;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.CacheSystem;

public class DatabankCache<T> where T : class, IDatabaseModel
{
    private ConcurrentDictionary<long, (T, long endLifeUTime)> CacheAutoBase { get; } = new();

    /// <summary>
    ///     Add or replace kv in cache.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="endLifeUTime"></param>
    public void AutoMod(T value, long endLifeUTime)
    {
        if (CacheAutoBase.ContainsKey(value.Id))
            CacheAutoBase[value.Id] = (value, endLifeUTime);
        else CacheAutoBase.TryAdd(value.Id, (value, endLifeUTime));
    }

    /// <summary>
    ///     Get tuple of tk from cache.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public (T, long endLifeUtime) AutoGet(long id)
    {
        return CacheAutoBase.GetValueOrDefault(id, (null, -1)!);
    }

    /// <summary>
    ///     Remove from cache.
    /// </summary>
    /// <param name="id"></param>
    public void AutoRemove(long id)
    {
        CacheAutoBase.TryRemove(id, out _);
    }

    /// <summary>
    ///     Get all cache elements without cloning.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public ConcurrentDictionary<long, (T, long endLifeUtime)> GetAll(out int count)
    {
        count = CacheAutoBase.Count;
        return CacheAutoBase;
    }
}