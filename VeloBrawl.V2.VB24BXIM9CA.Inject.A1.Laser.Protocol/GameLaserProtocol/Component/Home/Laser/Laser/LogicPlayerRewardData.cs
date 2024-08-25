using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;

public class LogicPlayerRewardData
{
    public void Encode(ByteStream byteStream)
    {
        new LogicRewardConfig().Encode(byteStream);

        byteStream.WriteVInt(0);
        byteStream.WriteBoolean(false);
    }
}