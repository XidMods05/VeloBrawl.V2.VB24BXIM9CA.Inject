using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;

public class BrawlPassSeasonData
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(6);
        byteStream.WriteVInt(0);
        byteStream.WriteBoolean(false);
        byteStream.WriteVInt(0);

        byteStream.WriteByte(2);
        byteStream.WriteInt(0);
        byteStream.WriteInt(0);
        byteStream.WriteInt(0);
        byteStream.WriteInt(0);

        byteStream.WriteByte(1);
        byteStream.WriteInt(0);
        byteStream.WriteInt(0);
        byteStream.WriteInt(0);
        byteStream.WriteInt(0);
    }
}