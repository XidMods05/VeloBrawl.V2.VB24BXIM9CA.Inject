using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.AutoMatch.Own;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.AutoMatch;

public class LogicServerCommand(ByteStream byteStream)
{
    public void Decode()
    {
        _ = byteStream.ReadVInt();
        new LogicCommand().Decode(byteStream);
    }

    public void Encode()
    {
        byteStream.WriteVInt(0);
        new LogicCommand().Encode(byteStream);
    }
}