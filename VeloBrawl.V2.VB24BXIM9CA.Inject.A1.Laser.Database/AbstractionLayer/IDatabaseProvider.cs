using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.CacheSystem;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.AbstractionLayer;

public interface IDatabaseProvider<T> : IDatabaseFirstSector where T : class
{
    public void AddOrReplace(T model, CacheLifeTime cacheLifeTime = CacheLifeTime.Big);

    public T? Get(long id, CacheLifeTime cacheLifeTime = CacheLifeTime.Big);
    public (T?, long endLifeUtime) GetInTuple(long id, CacheLifeTime cacheLifeTime = CacheLifeTime.Small);

    public bool Contains(long id);
    public void Remove(long id, bool delFromDatabase = false);

    public void Update(long id);
    public void Mutate();

    public T[] GetAll();
}