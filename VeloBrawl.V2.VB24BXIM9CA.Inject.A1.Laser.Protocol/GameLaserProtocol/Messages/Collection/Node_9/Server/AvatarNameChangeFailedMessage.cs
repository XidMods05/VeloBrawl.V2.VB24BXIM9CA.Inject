using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_9.Server;

public class AvatarNameChangeFailedMessage : PiranhaMessage
{
    public int Reason { get; set; }

    public override void Encode()
    {
        base.Encode();

        ByteStream.WriteInt(Reason);
    }

    public override int GetMessageType()
    {
        return 20205;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}