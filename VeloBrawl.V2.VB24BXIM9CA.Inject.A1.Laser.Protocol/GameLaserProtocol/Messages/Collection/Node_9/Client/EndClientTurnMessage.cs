using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.AutoMatch.Own;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_9.Client;

public class EndClientTurnMessage : PiranhaMessage
{
    public int Tick { get; set; }
    public int Checksum { get; set; }
    public List<LogicCommand> Commands { get; set; } = [];

    public override void Decode()
    {
        base.Decode();

        _ = ByteStream.ReadBoolean();

        Tick = ByteStream.ReadVInt();
        Checksum = ByteStream.ReadVInt();

        var commands = ByteStream.ReadVInt();

        for (var i = 0; i < commands; i++)
        {
            var command = LogicCommandManager.DecodeCommand(ByteStream);
            if (command != null) Commands.Add(command);
        }
    }

    public override int GetMessageType()
    {
        return 14102;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}