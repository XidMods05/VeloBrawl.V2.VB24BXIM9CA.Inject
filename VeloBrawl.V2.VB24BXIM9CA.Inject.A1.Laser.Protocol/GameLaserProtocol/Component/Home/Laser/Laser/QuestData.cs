using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector.HelpsLogic;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.MathematicalSector.GlobalId;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;

public class QuestData
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);

        byteStream.WriteVInt(0); // mission type. 1 = win in battle; 2 = kill players; 3 = to damage; 4 = to heal; 5 = play again; 6 = play in command.

        byteStream.WriteVInt(0); // now score
        byteStream.WriteVInt(0); // max score

        byteStream.WriteVInt(0); // tokens reward

        byteStream.WriteVInt(0);

        byteStream.WriteVInt(0); // now lvl
        byteStream.WriteVInt(0); // unk lvl

        byteStream.WriteVInt(0); // timer

        byteStream.WriteBoolean(false); // only pro pass
        byteStream.WriteBoolean(false); // quest seen

        ByteStreamHelper.WriteDataReference(byteStream, GlobalId.CreateGlobalId(16, 0)); // brawler globalId

        byteStream.WriteVInt(0); // game mode variation
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
    }
}