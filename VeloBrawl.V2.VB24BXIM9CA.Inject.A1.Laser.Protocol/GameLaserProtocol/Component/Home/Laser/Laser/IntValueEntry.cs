using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;

public class IntValueEntry(int x, int y)
{
    public int Key { get; } = x;
    public int Value { get; } = y;

    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteInt(Key);
        byteStream.WriteInt(Value);
    }
}