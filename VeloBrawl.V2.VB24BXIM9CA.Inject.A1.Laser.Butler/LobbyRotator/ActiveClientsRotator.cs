using System.Collections.Concurrent;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Butler.LobbyRotator;

public static class ActiveClientsRotator
{
    public static readonly ConcurrentDictionary<long, List<IAllocSession>> AllocSessions = new();
}