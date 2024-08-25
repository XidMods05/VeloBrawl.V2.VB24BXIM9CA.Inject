using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.AutoMatch;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.AutoMatch.Own;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.ServerCommands;

public class LogicChangeAvatarNameCommand(string newName, int changeNameCost) : LogicCommand
{
    public override void Encode(ByteStream byteStream)
    {
        byteStream.WriteString(newName);
        byteStream.WriteVInt(changeNameCost);

        new LogicServerCommand(byteStream).Encode();
    }

    public override void Decode(ByteStream byteStream)
    {
        byteStream.ReadString(1024);
        byteStream.ReadVInt();

        new LogicServerCommand(byteStream).Decode();
    }

    public override bool CanExecute(LogicHomeMode logicHomeMode)
    {
        return logicHomeMode.LogicClientAvatar.UseDiamonds(changeNameCost);
    }

    public override int Execute(LogicHomeMode logicHomeMode)
    {
        logicHomeMode.LogicClientAvatar.DatabaseModel.GetDocument().NameSetByUser = true;
        logicHomeMode.LogicClientAvatar.DatabaseModel.GetDocument().AvatarName = newName;
        return 0;
    }

    public override int GetCommandType()
    {
        return 201;
    }
}