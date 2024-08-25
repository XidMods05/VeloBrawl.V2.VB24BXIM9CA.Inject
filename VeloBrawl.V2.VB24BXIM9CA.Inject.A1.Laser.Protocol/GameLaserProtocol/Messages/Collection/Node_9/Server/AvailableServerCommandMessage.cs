using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.AutoMatch.Own;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_9.Server;

public class AvailableServerCommandMessage(LogicHomeMode logicHomeMode) : PiranhaMessage
{
    private LogicCommand _logicCommand = null!;
    private ILogicServerCommand _logicServerCommand = null!;

    public override void Encode()
    {
        base.Encode();

        if (_logicCommand != null!)
        {
            LogicCommandManager.EncodeCommand(_logicCommand, ByteStream, logicHomeMode);
            return;
        }

        LogicCommandManager.EncodeCommand(_logicServerCommand, ByteStream);
    }

    public void SetServerCommand(LogicCommand logicCommand)
    {
        _logicCommand = logicCommand;
    }

    public void SetServerCommand(ILogicServerCommand logicCommand)
    {
        _logicServerCommand = logicCommand;
    }

    public override int GetMessageType()
    {
        return 24111;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}