using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;

public class EventSlot(int counter)
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(counter);
    }
}