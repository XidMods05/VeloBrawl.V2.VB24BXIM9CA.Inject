using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.AutoMatch.Own;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Avatar;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_9.Server;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home;

public class LogicHomeMode
{
    public required IAllocSession AllocSession { get; set; } = null!;
    public required LogicClientHome LogicClientHome { get; set; }
    public required LogicClientAvatar LogicClientAvatar { get; set; }

    public int EndClientTurnReceived(int tick, int checksum, IEnumerable<LogicCommand> commands)
    {
        foreach (var outOfSyncMessage in from command in commands
                 where command.CanExecute(this)
                 where command.Execute(this) != 0
                 select new OutOfSyncMessage())
        {
            AllocSession.OwnerMessageManager.SendMessage(outOfSyncMessage);
            return -1;
        }

        return 0;
    }
}