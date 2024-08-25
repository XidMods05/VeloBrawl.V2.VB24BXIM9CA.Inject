using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Avatar;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_9.Server;

public class OwnHomeDataMessage : PiranhaMessage
{
    public LogicClientHome LogicClientHome { get; set; } = null!;
    public LogicClientAvatar LogicClientAvatar { get; set; } = null!;
    public int CurrentTimeInSecondsSinceEpoch { get; set; }

    public override void Encode()
    {
        base.Encode();

        LogicClientHome.Encode(ByteStream);
        LogicClientAvatar.Encode(ByteStream);

        ByteStream.WriteVInt(CurrentTimeInSecondsSinceEpoch);
    }

    public override int GetMessageType()
    {
        return 24101;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}