using System.Collections.Concurrent;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Server.Allocator.Alloc;

public static class SessionCollector
{
    public static ConcurrentDictionary<Guid, AllocSession> AllocSessions { get; } = new();
}