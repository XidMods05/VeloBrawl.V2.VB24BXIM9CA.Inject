using MongoDB.Bson;
using MongoDB.Driver;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.AbstractionLayer;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.CacheSystem;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.Database;

public class MongoDatabaseProvider<T> : IDatabaseProvider<T> where T : class, IDatabaseModel
{
    private readonly IMongoCollection<T> _collection;

    private readonly DatabankCache<T> _dbCache;
    private readonly object _provider = new();

    /// <summary>
    ///     DI supported constructor of MongoDB database.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="databaseCluster"></param>
    /// <param name="collection"></param>
    /// <param name="autoControlled">start default controller</param>
    /// <param name="secToUpdate">default seconds to update by default auto-controller (10min)</param>
    public MongoDatabaseProvider(string connectionString, string databaseCluster, string collection,
        bool autoControlled = true, int secToUpdate = 600)
    {
        _collection = new MongoClient(connectionString).GetDatabase(databaseCluster).GetCollection<T>(collection);
        _dbCache = new DatabankCache<T>();

        if (autoControlled)
            RunAutoControl(secToUpdate);
    }

    /// <summary>
    ///     Add model to database.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cacheLifeTime"></param>
    public void AddOrReplace(T model, CacheLifeTime cacheLifeTime = CacheLifeTime.Big)
    {
        lock (_provider)
        {
            if (model.Id < 0) model.Id = CountOfDocuments;

            if (!Contains(model.Id)) _collection.InsertOne(model);
            else _collection.ReplaceOne(x => x.Id == model.Id, model);

            _dbCache.AutoMod(model, cacheLifeTime.GetEndTime());
        }
    }

    /// <summary>
    ///     Get model from database.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cacheLifeTime"></param>
    /// <returns>
    ///     true if
    ///     <param name="id"></param>
    ///     contains in database, otherwise - false
    /// </returns>
    public T? Get(long id, CacheLifeTime cacheLifeTime = CacheLifeTime.Big)
    {
        if (_dbCache.AutoGet(id).Item1 != null!)
            return _dbCache.AutoGet(id).Item1;

        try
        {
            lock (_provider)
            {
                _dbCache.AutoMod(_collection.Find(x => x.Id == id).FirstOrDefault(), cacheLifeTime.GetEndTime());
                return _dbCache.AutoGet(id).Item1;
            }
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    ///     Get model from database in original tuple-visualisation.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cacheLifeTime"></param>
    /// <returns>
    ///     true if
    ///     <param name="id"></param>
    ///     contains in database, otherwise - false
    /// </returns>
    public (T?, long endLifeUtime) GetInTuple(long id, CacheLifeTime cacheLifeTime = CacheLifeTime.Small)
    {
        if (_dbCache.AutoGet(id).Item1 != null!)
            return _dbCache.AutoGet(id);

        try
        {
            lock (_provider)
            {
                _dbCache.AutoMod(_collection.Find(x => x.Id == id).FirstOrDefault(), cacheLifeTime.GetEndTime());
                return _dbCache.AutoGet(id);
            }
        }
        catch
        {
            return (null, -1);
        }
    }

    /// <summary>
    ///     Check for availability in database.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>true if ID contained, otherwise - false</returns>
    public bool Contains(long id)
    {
        lock (_provider)
        {
            return _collection.CountDocuments(x => x.Id == id) > 0;
        }
    }

    /// <summary>
    ///     Remove from database.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="andFromCollection"></param>
    public void Remove(long id, bool andFromCollection = false)
    {
        lock (_provider)
        {
            _dbCache.AutoRemove(id);

            if (andFromCollection)
                _collection.DeleteOne(x => x.Id == id);
        }
    }

    /// <summary>
    ///     Safe model update.
    /// </summary>
    /// <param name="id"></param>
    public void Update(long id)
    {
        lock (_provider)
        {
            var value = GetInTuple(id);
            if (value.Item1 == null) return;

            value.Item1.SecureSaveOfMutations();
            _collection.ReplaceOne(x => x.Id == id, value.Item1);

            if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() >= value.endLifeUtime) _dbCache.AutoRemove(id);
        }
    }

    /// <summary>
    ///     Parallel update of models.
    /// </summary>
    public void Mutate()
    {
        if (_dbCache == null!) return;
        _dbCache.GetAll(out _).AsParallel().ForAll(data => Update(data.Key));
    }

    /// <summary>
    ///     Get all data from database.
    /// </summary>
    /// <returns></returns>
    public T[] GetAll()
    {
        var count = CountOfDocuments;

        var list = new List<T>();
        {
            lock (_provider)
            {
                for (var i = 0; i < count; i++)
                {
                    var d = Get(i);
                    if (d != null) list.Add(d);
                }
            }
        }

        return list.ToArray();
    }

    /// <summary>
    ///     Returns count of documents.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public int CountOfDocuments
    {
        get
        {
            lock (_provider)
            {
                return (int)_collection.CountDocuments(new BsonDocument());
            }
        }
        set => throw new NotImplementedException();
    }

    /// <summary>
    ///     Control system of mutations.
    /// </summary>
    public void RunAutoControl(int secondsOfControl)
    {
        new Thread(() =>
        {
            while (true)
            {
                Mutate();
                Thread.Sleep(1000 * secondsOfControl);
            }
        }).Start();
    }
}