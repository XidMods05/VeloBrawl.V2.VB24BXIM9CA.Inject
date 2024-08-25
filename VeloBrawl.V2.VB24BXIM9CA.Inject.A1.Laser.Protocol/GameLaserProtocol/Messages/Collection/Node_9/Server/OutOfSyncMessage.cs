using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_9.Server;

public class OutOfSyncMessage : PiranhaMessage
{
    public override void Encode()
    {
        base.Encode();

        ByteStream.WriteVInt(0);
        ByteStream.WriteVInt(0);
        ByteStream.WriteVInt(0);
    }

    public override int GetMessageType()
    {
        return 24104;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}