using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;

public class CustomEvent
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);

        new ChronosTextEntry("").Encode(byteStream);
        new ChronosTextEntry("").Encode(byteStream);
        new ChronosTextEntry("").Encode(byteStream);
    }
}