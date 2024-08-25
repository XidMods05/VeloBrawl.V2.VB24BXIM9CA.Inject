using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;

public class VanityItems(List<VanityItemEntry> vanityItemEntries)
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(vanityItemEntries.Count);
        {
            if (vanityItemEntries.Count <= 0) return;

            foreach (var vanityItemEntry in vanityItemEntries) vanityItemEntry.Encode(byteStream);
        }
    }
}