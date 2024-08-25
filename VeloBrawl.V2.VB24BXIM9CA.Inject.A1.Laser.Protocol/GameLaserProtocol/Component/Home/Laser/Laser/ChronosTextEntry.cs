using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;

public class ChronosTextEntry(string text, bool outOfCsv = false)
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteInt(outOfCsv ? 1 : 0);
        byteStream.WriteStringReference(text);
    }
}