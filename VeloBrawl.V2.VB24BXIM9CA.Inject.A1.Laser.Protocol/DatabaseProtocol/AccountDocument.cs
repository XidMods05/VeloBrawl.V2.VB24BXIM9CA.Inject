using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging.LangManager;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.DatabaseProtocol;

[Serializable]
public class AccountDocument
{
    public long AccountId { get; set; }
    public int HomeId { get; set; }
    public string PassToken { get; set; } = string.Empty;

    public string Device { get; set; } = string.Empty;
    public string StartDevice { get; set; } = string.Empty;

    public int StartPlayTimeSeconds { get; set; }
    public int PlayTimeSeconds { get; set; }

    public string AccountCreatedDate { get; set; } = string.Empty;
    public int SessionsCount { get; set; }

    public LanguageManager LanguageManager { get; set; } = LanguageManager.Chinese;

    public int LastActiveUnixTime { get; set; }
    public bool IsOnline { get; set; }

    public int Trophies { get; set; }
    public int MaxTrophies { get; set; }
    public int TrophyRoadProgress { get; set; }
    public int Experience { get; set; }

    public int Diamonds { get; set; }
    public int UpgradeMaterial { get; set; }
    public int StarPoints { get; set; }
    public int TokensDoubler { get; set; }
    public int ClubCoins { get; set; }

    public int SoloWins { get; set; }
    public int DuoWins { get; set; }
    public int TrioWins { get; set; }
    public int RoboRumbleWins { get; set; }
    public int BossFightWins { get; set; }
    public int GodzillaWins { get; set; }
    public int ChallengeWins { get; set; }

    public int SoloLeague { get; set; }
    public int TeamLeague { get; set; }
    public int AllianceLeague { get; set; }

    public int HomeBrawlerGid { get; set; }
    public int PlayerStatus { get; set; }

    public int ThumbnailDataGid { get; set; }
    public int NameColorDataGid { get; set; }

    public string AvatarName { get; set; } = "Brawler";
    public bool NameSetByUser { get; set; }
    public int NumberOfNameChanges { get; set; }
}