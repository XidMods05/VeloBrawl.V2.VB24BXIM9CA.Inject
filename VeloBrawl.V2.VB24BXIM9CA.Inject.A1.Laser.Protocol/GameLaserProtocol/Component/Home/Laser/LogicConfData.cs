using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Butler.LobbyRotator;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.Database.Newbie;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.DatabaseProtocol;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.TablesOfDataSector;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser;

public class LogicConfData(DatabaseModel<AccountDocument> accountModel)
{
    public void Encode(ByteStream byteStream)
    {
        int[] v2 = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 20, 21, 22, 23, 24];

        byteStream.WriteVInt(0);

        byteStream.WriteVInt(v2.Length);
        {
            foreach (var x in v2) new EventSlot(x).Encode(byteStream);
        }

        byteStream.WriteVInt(VisibilityRotator.Event1DataMassive.Count);
        {
            foreach (var variable in VisibilityRotator.Event1DataMassive)
                variable.Value.Encode(byteStream);
        }

        byteStream.WriteVInt(VisibilityRotator.Event2DataMassive.Count);
        {
            foreach (var variable in VisibilityRotator.Event2DataMassive)
                variable.Value.Encode(byteStream);
        }

        var v101 = byteStream.WriteVInt(1);
        {
            for (var i = 0; i < v101; i++) byteStream.WriteVInt(i);
        }

        var v102 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v102; i++) byteStream.WriteVInt(i);
        }

        var v103 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v103; i++) byteStream.WriteVInt(i);
        }

        byteStream.WriteBoolean(false);

        var v104 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v104; i++) new ReleaseEntry().Encode(byteStream);
        }

        byteStream.WriteVInt(1);
        {
            var d = LogicDataTables.GetAllDataFromCsvById(41);
            {
                Random.Shared.Shuffle(d);
            }

            new IntValueEntry(1, d[1].GetGlobalId()).Encode(byteStream);
        }

        var v105 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v105; i++) new TimedIntValueEntry().Encode(byteStream);
        }

        var v106 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v106; i++) new CustomEvent().Encode(byteStream);
        }
    }
}