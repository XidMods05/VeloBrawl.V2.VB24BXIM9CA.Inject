using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.MathematicalSector.Types;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_1.Server;

public class LoginOkMessage : PiranhaMessage
{
    public string? AccountCreatedDate { get; set; }
    public long AccountId { get; set; }
    public int ContentVersion { get; set; }
    public int DaysSinceStartedPlaying { get; set; }
    public string? FacebookId { get; set; }
    public string? GamecenterId { get; set; }
    public long HomeId { get; set; }
    public string? KunlunId { get; set; }
    public string? LoginCountry { get; set; }
    public string? PassToken { get; set; }
    public int PlayTimeSeconds { get; set; }
    public int SecondsUntilAccountDeletion { get; set; }
    public int ServerBuild { get; set; }
    public string? ServerEnvironment { get; set; }
    public int ServerMajorVersion { get; set; }
    public string? ServerTime { get; set; }
    public int SessionCount { get; set; }
    public int StartupCooldownSeconds { get; set; }
    public int Tier { get; set; }

    public override void Encode()
    {
        base.Encode();

        new LogicLong(0, (int)AccountId).Encode(ByteStream);
        new LogicLong(0, (int)HomeId).Encode(ByteStream);
        ByteStream.WriteString(PassToken);
        ByteStream.WriteString(FacebookId);
        ByteStream.WriteString(GamecenterId);
        ByteStream.WriteInt(ServerMajorVersion);
        ByteStream.WriteInt(ContentVersion);
        ByteStream.WriteInt(ServerBuild);
        ByteStream.WriteString(ServerEnvironment);
        ByteStream.WriteInt(SessionCount);
        ByteStream.WriteInt(PlayTimeSeconds);
        ByteStream.WriteInt(DaysSinceStartedPlaying);
        ByteStream.WriteString(ServerTime);
        ByteStream.WriteString(AccountCreatedDate);
        ByteStream.WriteString(null);
        ByteStream.WriteInt(StartupCooldownSeconds);
        ByteStream.WriteString(null);
        ByteStream.WriteString(LoginCountry);
        ByteStream.WriteString(KunlunId);
        ByteStream.WriteInt(Tier);
        ByteStream.WriteInt(0);
        ByteStream.WriteInt(SecondsUntilAccountDeletion);
        ByteStream.WriteInt(1);
        ByteStream.WriteString(null);
        ByteStream.WriteInt(1);
        ByteStream.WriteInt(1);
        ByteStream.WriteBoolean(true);
    }

    public override int GetMessageType()
    {
        return 20104;
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }
}