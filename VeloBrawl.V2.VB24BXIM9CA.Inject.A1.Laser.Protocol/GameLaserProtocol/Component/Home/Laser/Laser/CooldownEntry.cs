using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector.HelpsLogic;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;

public class CooldownEntry
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(0);
        ByteStreamHelper.WriteDataReference(byteStream, 0);
        byteStream.WriteVInt(0);
    }
}