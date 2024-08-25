using System.Collections.Concurrent;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.AttachmentProtocol;

public static class LogicHomeModeCollector
{
    public static readonly ConcurrentDictionary<long, LogicHomeMode> LogicHomeModes = new();
}