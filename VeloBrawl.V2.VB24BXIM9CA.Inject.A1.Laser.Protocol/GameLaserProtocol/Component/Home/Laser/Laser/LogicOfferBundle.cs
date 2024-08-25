using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.MathematicalSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.Enumerations.Game;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.InGameUtilities;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;

public class LogicOfferBundle(
    List<LogicGemOffer> logicGemOffers,
    string offerTitle,
    string backgroundTheme,
    ShopPriceTypeHelperTable shopPriceTypeHelperTable,
    int offerPrice,
    int oldOfferPrice,
    int endTime,
    bool isInDailyOffers,
    bool confirmPurchase)
{
    private bool _purchased;

    public void Encode(ByteStream byteStream)
    {
        var v4 = 1;

        var v5E = oldOfferPrice > offerPrice;
        var v5X = 0;
        var v5Y = 0;

        if (offerPrice != 0 && oldOfferPrice != 0)
        {
            if (v5E)
            {
                var v5L = LogicMath.Abs(LogicMath.Max(oldOfferPrice, offerPrice) /
                                        LogicMath.Min(oldOfferPrice, offerPrice));

                v4 = v5L % 10 != 0 && v5L > 1 ? 1 : 2;

                v5E = oldOfferPrice > offerPrice;
                v5X = LogicMath.Max(oldOfferPrice, offerPrice) / LogicMath.Min(oldOfferPrice, offerPrice);
                v5Y = LogicMath.Abs((int)(oldOfferPrice * 100f / offerPrice));

                if (v4 == 2)
                    if (v5Y > 300)
                        v4--;
            }

            if (v5Y > 0)
            {
                v4 = LogicMath.Clamp(v4, 0, 2);
                v5X = LogicMath.Clamp(v5X, 2, 500);
                v5Y = LogicMath.Clamp(v5Y, 1, 1000);
            }
        }

        var v6 = v4 == 1 ? v5X > 10 : v5Y > 500 / 2;

        byteStream.WriteVInt(logicGemOffers.Count);
        {
            foreach (var logicGemOffer in logicGemOffers) logicGemOffer.Encode(byteStream);
        }

        byteStream.WriteVInt((int)shopPriceTypeHelperTable);
        byteStream.WriteVInt(offerPrice);

        byteStream.WriteVInt(LogicMath.Max(LogicTimeUtil.GetTimestamp() - endTime, 0));
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteBoolean(_purchased);

        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);

        byteStream.WriteBoolean(isInDailyOffers);
        byteStream.WriteVInt(oldOfferPrice);

        new ChronosTextEntry(offerTitle).Encode(byteStream);

        byteStream.WriteVInt(confirmPurchase);
        byteStream.WriteString(backgroundTheme);
        byteStream.WriteVInt(0);
        byteStream.WriteBoolean(false);

        byteStream.WriteVInt(v5E ? byteStream.WriteVInt(v4) == 1 ? v5X : v5Y : byteStream.WriteVInt(0));
    }

    public bool GetPurchased()
    {
        return _purchased;
    }

    public void SetPurchased(bool value)
    {
        _purchased = value;
    }
}