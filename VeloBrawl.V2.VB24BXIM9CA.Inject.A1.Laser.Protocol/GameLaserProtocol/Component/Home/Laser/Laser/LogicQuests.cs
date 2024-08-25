using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;

public class LogicQuests(List<QuestData> questDataList)
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(questDataList.Count);
        {
            if (questDataList.Count <= 0) return;

            foreach (var questData in questDataList) questData.Encode(byteStream);
        }
    }
}