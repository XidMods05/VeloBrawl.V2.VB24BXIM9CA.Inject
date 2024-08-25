using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_1.Server;

public class KeepAliveServerMessage : PiranhaMessage
{
    public override int GetMessageType()
    {
        return 20108;
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }
}