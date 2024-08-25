namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.CacheSystem;

public enum CacheLifeTime : byte
{
    /// <summary>
    ///     Lifetime in cache = 5 minutes.
    /// </summary>
    Small = 0,

    /// <summary>
    ///     Lifetime in cache = 16 hours.
    /// </summary>
    Big = 1,

    // <<< special sector >>>

    /// <summary>
    ///     Lifetime in cache = 60 seconds.
    /// </summary>
    Nano = 2,

    /// <summary>
    ///     Lifetime in cache = 90 minutes.
    /// </summary>
    Normal = 3,

    /// <summary>
    ///     Lifetime in cache = 36 hours.
    /// </summary>
    VeryLong = 4,

    /// <summary>
    ///     Lifetime in cache = 240 hours.
    /// </summary>
    VeryVeryVeryLong = 5
}

public static class CacheLifeTimeExtensions
{
    /// <summary>
    ///     Get end-lifetime in unix format.
    /// </summary>
    /// <param name="cacheLifeTime"></param>
    /// <returns></returns>
    public static long GetEndTime(this CacheLifeTime cacheLifeTime)
    {
        var v1 = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        return cacheLifeTime switch
        {
            CacheLifeTime.Nano => v1 + 1 * 60, // 2
            CacheLifeTime.Normal => v1 + 1 * 60 * 90, // 3
            CacheLifeTime.VeryLong => v1 + 1 * 60 * 60 * 36, // 4
            CacheLifeTime.VeryVeryVeryLong => v1 + 1 * 60 * 60 * 240, // 5
            _ => v1 + 1 * 60 * (5 + 55 * (int)cacheLifeTime) * (1 + 15 * (int)cacheLifeTime) // 0 and 1
        };
    }
}