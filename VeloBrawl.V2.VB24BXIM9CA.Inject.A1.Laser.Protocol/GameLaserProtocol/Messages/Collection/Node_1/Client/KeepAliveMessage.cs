using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_1.Client;

public class KeepAliveMessage : PiranhaMessage
{
    public override int GetMessageType()
    {
        return 10108;
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }
}