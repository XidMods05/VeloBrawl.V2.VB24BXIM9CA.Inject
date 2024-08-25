using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector.HelpsLogic;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.MathematicalSector.Types;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.MessagingSector.Messages;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_1.Client;

public class LoginMessage : PiranhaMessage
{
    public long AccountId { get; set; }
    public int ClientBuild { get; set; }
    public int ClientMajorVersion { get; set; }
    public int ClientMinor { get; set; }
    public string ClientVersion { get; set; } = string.Empty;
    public string Device { get; set; } = string.Empty;
    public string Imei { get; set; } = string.Empty;
    public bool IsAdvertisingEnabled { get; set; }
    public bool IsAndroid { get; set; }
    public string OsVersion { get; set; } = string.Empty;
    public string PassToken { get; set; } = string.Empty;
    public string PreferredDeviceLanguage { get; set; } = string.Empty;
    public int PreferredLanguage { get; set; }
    public string ResourceSha { get; set; } = string.Empty;
    public int RndKey { get; set; }

    public override void Decode()
    {
        base.Decode();

        AccountId = new LogicLong().Decode(ByteStream);
        PassToken = ByteStream.ReadString(1024);
        ClientMajorVersion = ByteStream.ReadInt();
        ClientMinor = ByteStream.ReadInt();
        ClientBuild = ByteStream.ReadInt();
        ResourceSha = ByteStream.ReadString(1024);
        Device = ByteStream.ReadString(1024);
        PreferredLanguage = ByteStreamHelper.ReadDataReference(ByteStream);
        PreferredDeviceLanguage = ByteStream.ReadString(1024);
        OsVersion = ByteStream.ReadString(1024);
        IsAndroid = ByteStream.ReadBoolean();
        Imei = ByteStream.ReadStringReference(1024);
        _ = ByteStream.ReadStringReference(1024);
        IsAdvertisingEnabled = ByteStream.ReadBoolean();
        _ = ByteStream.ReadString(1024);
        RndKey = ByteStream.ReadInt();
        _ = ByteStream.ReadVInt();
        ClientVersion = ByteStream.ReadStringReference(1024);
    }

    public override int GetMessageType()
    {
        return TitanLoginMessage.GetMessageType();
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }
}