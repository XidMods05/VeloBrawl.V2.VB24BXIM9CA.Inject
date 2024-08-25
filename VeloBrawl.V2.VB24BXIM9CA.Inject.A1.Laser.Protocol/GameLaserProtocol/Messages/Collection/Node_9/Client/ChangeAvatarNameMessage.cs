using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_9.Client;

public class ChangeAvatarNameMessage : PiranhaMessage
{
    public string Name { get; set; } = "Brawler";
    public bool NameSetByUser { get; set; }

    public override void Decode()
    {
        base.Decode();

        Name = ByteStream.ReadString();
        NameSetByUser = ByteStream.ReadBoolean();
    }

    public override int GetMessageType()
    {
        return 10212;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}