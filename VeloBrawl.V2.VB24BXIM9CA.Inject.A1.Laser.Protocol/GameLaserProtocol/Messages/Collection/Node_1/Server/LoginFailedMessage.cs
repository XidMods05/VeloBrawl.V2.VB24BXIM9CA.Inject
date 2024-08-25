using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.MessagingSector.Messages;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_1.Server;

public class LoginFailedMessage : PiranhaMessage
{
    public string ContentUrl { get; set; } = string.Empty;
    public int ErrorCode { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string RedirectDomain { get; set; } = string.Empty;
    public string RemoveResourceFingerprintData { get; set; } = string.Empty;
    public int SecondsUntilMaintenanceEnd { get; set; }
    public bool ShowContactSupportForBan { get; set; }
    public string UpdateUrl { get; set; } = string.Empty;

    public override void Encode()
    {
        base.Encode();

        ByteStream.WriteInt(ErrorCode);
        ByteStream.WriteString(RemoveResourceFingerprintData);
        ByteStream.WriteString(RedirectDomain);
        ByteStream.WriteString(ContentUrl);
        ByteStream.WriteString(UpdateUrl);
        ByteStream.WriteString(Reason);
        ByteStream.WriteInt(SecondsUntilMaintenanceEnd);
        ByteStream.WriteBoolean(ShowContactSupportForBan);
    }

    public override int GetMessageType()
    {
        return TitanLoginFailedMessage.GetMessageType();
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }
}