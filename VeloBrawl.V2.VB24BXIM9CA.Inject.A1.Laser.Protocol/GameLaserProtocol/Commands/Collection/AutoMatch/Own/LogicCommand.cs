using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.AutoMatch.Own;

public class LogicCommand
{
    public virtual void Decode(ByteStream byteStream)
    {
        byteStream.ReadVInt();
        byteStream.ReadVInt();
        byteStream.ReadVInt();
        byteStream.ReadVInt();
    }

    public virtual void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(1);
        byteStream.WriteVInt(1);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
    }

    public virtual bool CanExecute(LogicHomeMode logicHomeMode)
    {
        return false;
    }

    public virtual int Execute(LogicHomeMode logicHomeMode)
    {
        return 0;
    }

    public virtual int GetCommandType()
    {
        return 0;
    }
}