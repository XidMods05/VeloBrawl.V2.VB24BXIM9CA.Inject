using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Butler.SharedProtocol;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Butler.LobbyRotator;

public static class VisibilityRotator
{
    public static string FilePath = null!;
    public static long DefaultSecTimeForEventUpdate { get; set; }
    public static Dictionary<int, EventData> Event1DataMassive { get; set; } = new();
    public static Dictionary<int, EventData> Event2DataMassive { get; set; } = new();
}