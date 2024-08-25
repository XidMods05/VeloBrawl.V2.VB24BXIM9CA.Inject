using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.Database.Newbie;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.DatabaseProtocol;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector.HelpsLogic;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.MathematicalSector.GlobalId;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.HelpDirectory;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.InGameUtilities;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging.LangManager;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser;

public class LogicDailyData(DatabaseModel<AccountDocument> accountModel)
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(LogicTimeUtil.GetDayIndex());
        byteStream.WriteVInt(LogicTimeUtil.GetTimeOfDay());

        byteStream.WriteVInt(accountModel.GetDocument().Trophies); // trophies
        byteStream.WriteVInt(accountModel.GetDocument().MaxTrophies); // max trophies
        byteStream.WriteVInt(accountModel.GetDocument().MaxTrophies); // max trophies
        byteStream.WriteVInt(accountModel.GetDocument().TrophyRoadProgress + 1); // max trophy-road progress
        byteStream.WriteVInt(accountModel.GetDocument().Experience); // experience

        ByteStreamHelper.WriteDataReference(byteStream, accountModel.GetDocument().ThumbnailDataGid);
        ByteStreamHelper.WriteDataReference(byteStream, accountModel.GetDocument().NameColorDataGid);

        var v101 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v101; i++) byteStream.WriteVInt(i);
        }

        var v102 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v102; i++) ByteStreamHelper.WriteDataReference(byteStream, 0, 0);
        }

        var v103 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v103; i++)
            {
                byteStream.WriteVInt(i);
                ByteStreamHelper.WriteDataReference(byteStream, 0, 0);
            }
        }

        var v104 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v104; i++) ByteStreamHelper.WriteDataReference(byteStream, 0, 0);
        }

        var v105 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v105; i++) ByteStreamHelper.WriteDataReference(byteStream, 0, 0);
        }

        var v106 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v106; i++) ByteStreamHelper.WriteDataReference(byteStream, 0, 0);
        }

        byteStream.WriteVInt(0);
        byteStream.WriteVInt(accountModel.GetDocument().MaxTrophies);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);

        byteStream.WriteBoolean(true);

        byteStream.WriteVInt(accountModel.GetDocument().TokensDoubler);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);

        new ForcedDrops().Encode(byteStream);

        if (byteStream.WriteBoolean(false))
            new TimedOffer().Encode(byteStream);

        if (byteStream.WriteBoolean(false))
            new TimedOffer().Encode(byteStream);

        byteStream.WriteBoolean(false);

        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);

        byteStream.WriteVInt(HelperCity.GetChangeNameCostByCount(accountModel.GetDocument().NumberOfNameChanges));

        byteStream.WriteVInt(0); // ---
        /*byteStream.WriteVInt(LogicMath.Max(
            Convert.ToInt32(
                accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(
                    AccountStructure.NameChangeEndTime)) - LogicTimeUtil.GetTimestamp(), 0));*/

        byteStream.WriteVInt(0);

        byteStream.WriteVInt(0); // offers
        {
            //foreach (var v1L in v1) v1L.Encode(byteStream);
        }

        var v107 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v107; i++) new AdStatus().Encode(byteStream);
        }

        byteStream.WriteVInt(0); // available battle tokens;
        byteStream.WriteVInt(-1);

        var v108 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v108; i++) byteStream.WriteVInt(i);
        }

        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);

        ByteStreamHelper.WriteDataReference(byteStream, GlobalId.CreateGlobalId(16, 0));

        byteStream.WriteString(accountModel.GetDocument().LanguageManager.GetTextCodeByLanguage().ToUpper());
        byteStream.WriteString("ZaRussia");

        var v109 = byteStream.WriteVInt(0); // invalueentry x
        {
            /*if (v109 > 0)
                foreach (var v109L in v3)
                    v109L.Value.Encode(byteStream);*/
        }

        var v110 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v110; i++) new CooldownEntry().Encode(byteStream);
        }

        var v111 = byteStream.WriteVInt(1);
        {
            for (var i = 0; i < v111; i++) new BrawlPassSeasonData().Encode(byteStream);
        }

        var v112 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v112; i++) new ProLeagueSeasonData().Encode(byteStream);
        }

        if (byteStream.WriteBoolean(true)) new LogicQuests([]).Encode(byteStream);

        if (byteStream.WriteBoolean(true)) new VanityItems([]).Encode(byteStream);

        if (byteStream.WriteBoolean(true)) new LogicPlayerRankedSeasonData().Encode(byteStream);

        byteStream.WriteInt(0);
    }
}