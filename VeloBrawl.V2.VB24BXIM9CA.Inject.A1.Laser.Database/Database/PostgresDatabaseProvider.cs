using Newtonsoft.Json;
using Npgsql;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.AbstractionLayer;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.CacheSystem;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.Database;

public class PostgresDatabaseProvider<T> : IDatabaseProvider<T> where T : class, IDatabaseModel
{
    private readonly DatabankCache<T> _dbCache;
    private readonly NpgsqlConnection _npgsqlConnection;

    private readonly object _provider;
    private readonly string _tableName;

    /// <summary>
    ///     DI supported constructor of PostgresSQL database.
    /// </summary>
    /// <param name="connectionString">PostgresSQL connection string</param>
    /// <param name="tableName">PostgresSQL table name</param>
    /// <param name="autoControlled">start default controller</param>
    /// <param name="secToUpdate">default seconds to update by default auto-controller (10min)</param>
    public PostgresDatabaseProvider(string connectionString, string tableName, bool autoControlled = true,
        int secToUpdate = 600)
    {
        _npgsqlConnection = new NpgsqlConnection(connectionString);
        _dbCache = new DatabankCache<T>();

        _provider = new object();
        _tableName = tableName;

        _npgsqlConnection.Open();
        {
            CreateTableIfNotExists();
        }

        if (autoControlled)
            RunAutoControl(secToUpdate);
    }

    /// <summary>
    ///     Count of documents in the database.
    /// </summary>
    public int CountOfDocuments
    {
        get
        {
            lock (_provider)
            {
                using var command = new NpgsqlCommand($"SELECT COUNT(*) FROM {_tableName}", _npgsqlConnection);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
        set => throw new NotImplementedException();
    }

    /// <summary>
    ///     Add or replace a document in the database.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cacheLifeTime"></param>
    public void AddOrReplace(T model, CacheLifeTime cacheLifeTime = CacheLifeTime.VeryLong)
    {
        if (model.Id < 0) model.Id = CountOfDocuments;

        lock (_provider)
        {
            if (Contains(model.Id))
            {
                Update(model.Id);
                return;
            }

            using var command = new NpgsqlCommand($"INSERT INTO {_tableName} (id, data) VALUES (@id, @data)",
                _npgsqlConnection);
            {
                command.Parameters.AddWithValue("@id", model.Id);
                command.Parameters.AddWithValue("@data", JsonConvert.SerializeObject(model));
            }

            command.ExecuteNonQuery();
            _dbCache.AutoMod(model, cacheLifeTime.GetEndTime());
        }
    }

    /// <summary>
    ///     Get a document from the database.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cacheLifeTime"></param>
    /// <returns></returns>
    public T? Get(long id, CacheLifeTime cacheLifeTime = CacheLifeTime.Big)
    {
        if (_dbCache.AutoGet(id).Item1 != null!)
            return _dbCache.AutoGet(id).Item1;

        lock (_provider)
        {
            using var command =
                new NpgsqlCommand($"SELECT id, data FROM {_tableName} WHERE id = @id", _npgsqlConnection);
            {
                command.Parameters.AddWithValue("@id", id);
            }

            using var reader = command.ExecuteReader();
            if (!reader.Read()) return null;

            var m = JsonConvert.DeserializeObject<T>(reader.GetValue(1).ToString()!);

            _dbCache.AutoMod(m!, cacheLifeTime.GetEndTime());
            return m;
        }
    }

    /// <summary>
    ///     Get a document from the database and return tuple with document and end-lifetime in unix format.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cacheLifeTime"></param>
    /// <returns></returns>
    public (T?, long endLifeUtime) GetInTuple(long id, CacheLifeTime cacheLifeTime = CacheLifeTime.Big)
    {
        if (_dbCache.AutoGet(id).Item1 != null!)
            return _dbCache.AutoGet(id);

        lock (_provider)
        {
            using var command =
                new NpgsqlCommand($"SELECT id, data FROM {_tableName} WHERE id = @id", _npgsqlConnection);
            {
                command.Parameters.AddWithValue("@id", id);
            }

            using var reader = command.ExecuteReader();
            if (!reader.Read()) return (null, 0);

            var m = JsonConvert.DeserializeObject<T>(reader.GetValue(1).ToString()!);
            var e = cacheLifeTime.GetEndTime();

            _dbCache.AutoMod(m!, e);
            return (m, e);
        }
    }

    /// <summary>
    ///     Check if a document exists in the database.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Contains(long id)
    {
        if (_dbCache.AutoGet(id).Item1 != null!)
            return true;

        lock (_provider)
        {
            using var command = new NpgsqlCommand($"SELECT EXISTS(SELECT 1 FROM {_tableName} WHERE id = @id)",
                _npgsqlConnection);
            {
                command.Parameters.AddWithValue("@id", id);
            }

            return (bool)command.ExecuteScalar()!;
        }
    }

    /// <summary>
    ///     Remove a document from the database.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="delFromDatabase">can be deleted from the database</param>
    public void Remove(long id, bool delFromDatabase = false)
    {
        _dbCache.AutoRemove(id);
        if (!delFromDatabase) return;

        lock (_provider)
        {
            using var command = new NpgsqlCommand($"DELETE FROM {_tableName} WHERE id = @id", _npgsqlConnection);
            {
                command.Parameters.AddWithValue("@id", id);
            }

            command.ExecuteNonQuery();
        }
    }

    /// <summary>
    ///     Update a document in the database.
    /// </summary>
    /// <param name="id"></param>
    public void Update(long id)
    {
        var value = GetInTuple(id);
        if (value.Item1 == null) return;

        value.Item1.SecureSaveOfMutations();

        lock (_provider)
        {
            using var command =
                new NpgsqlCommand($"UPDATE {_tableName} SET data = @data WHERE id = @id", _npgsqlConnection);
            {
                command.Parameters.AddWithValue("@id", value.Item1.Id);
                command.Parameters.AddWithValue("@data", JsonConvert.SerializeObject(value.Item1));
            }

            command.ExecuteNonQuery();
        }

        if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() >= value.endLifeUtime)
            _dbCache.AutoRemove(id);
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
        var c = CountOfDocuments;

        var list = new List<T>();
        {
            lock (_provider)
            {
                for (var i = 0; i < c; i++)
                {
                    var d = Get(i);
                    if (d != null) list.Add(d);
                }
            }
        }

        return list.ToArray();
    }

    /// <summary>
    ///     Create table if not exists.
    /// </summary>
    public void CreateTableIfNotExists()
    {
        lock (_provider)
        {
            var createTableQuery = $"""
                                    
                                                CREATE TABLE IF NOT EXISTS {_tableName} (
                                                    id BIGINT PRIMARY KEY,
                                                    data TEXT NOT NULL
                                                );
                                    """;

            using var command = new NpgsqlCommand(createTableQuery, _npgsqlConnection);
            command.ExecuteNonQuery();
        }
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