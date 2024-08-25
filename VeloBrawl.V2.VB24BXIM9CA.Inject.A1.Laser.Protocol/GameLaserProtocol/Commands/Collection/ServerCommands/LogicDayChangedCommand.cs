using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.AutoMatch;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.AutoMatch.Own;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.ServerCommands;

public class LogicDayChangedCommand(LogicConfData logicConfData) : LogicCommand
{
    public override void Encode(ByteStream byteStream)
    {
        if (byteStream.WriteBoolean(true)) logicConfData.Encode(byteStream);
        new LogicServerCommand(byteStream).Encode();
    }

    public override int GetCommandType()
    {
        return 204;
    }
}