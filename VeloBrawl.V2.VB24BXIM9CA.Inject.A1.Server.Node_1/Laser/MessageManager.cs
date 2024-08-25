using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Reflection;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.DependencyLayer.P1.DI_P1;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Butler.LobbyRotator;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.Database.Newbie;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.AttachmentProtocol;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.DatabaseProtocol;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Avatar;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_1.Client;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_1.Server;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_9.Server;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.MathematicalSector.GlobalId;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.Enumerations.Extension;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.Enumerations.Game;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.HelpDirectory;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.InGameUtilities;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging.LangManager;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Server.Node_1.Laser;

public class MessageManager : IMessageManager
{
    private readonly FrozenDictionary<int, (MethodInfo, Type)> _collection;

    private readonly IOwnerMessageManager _ownerMessageManager;

    private DatabaseModel<AccountDocument> _databaseModel = null!;
    private LogicHomeMode _logicHomeMode = null!;

    public MessageManager(IAllocSession allocSession, IOwnerMessageManager ownerMessageManager)
    {
        AllocSession = allocSession;

        var cd = new ConcurrentDictionary<int, (MethodInfo, Type)>();
        {
            Parallel.ForEach(GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance), (method, _) =>
            {
                var parameters = method.GetParameters();
                if (parameters.Length < 1 || !parameters[0].ParameterType.IsSubclassOf(typeof(PiranhaMessage))) return;

                cd.TryAdd(((PiranhaMessage)Activator.CreateInstance(parameters[0].ParameterType)!).GetMessageType(),
                    (method, parameters[0].ParameterType));
            });
        }

        _ownerMessageManager = ownerMessageManager;
        _collection = cd.ToFrozenDictionary();
    }

    public IAllocSession AllocSession { get; set; }

    public void SetAccountModel(long databaseId)
    {
        _databaseModel = DependencyHelperP1.GetAccountInject().Get(databaseId)!;
    }

    public int ReceiveMessage(IPiranhaMessage piranhaMessage)
    {
        if (_collection.TryGetValue(piranhaMessage.GetMessageType(), out var value))
            return Convert.ToInt32(value.Item1.Invoke(this, [Convert.ChangeType(piranhaMessage, value.Item2)]));
        return -6002;
    }

    public int SendMessage(IPiranhaMessage piranhaMessage)
    {
        if (!piranhaMessage.IsServerToClientMessage()) return -7001;
        return _ownerMessageManager.SendMessage(piranhaMessage);
    }

    public void Goodbye()
    {
        if (_databaseModel == null!) return;
        _databaseModel.Document.IsOnline = false;
        LogicHomeModeCollector.LogicHomeModes.TryRemove(_databaseModel.Id, out _);
        ActiveClientsRotator.AllocSessions.GetValueOrDefault(_databaseModel.Id)?.Remove(AllocSession);
    }

    // ReSharper disable once UnusedMember.Local
    private int KeepAliveMessageReceived(KeepAliveMessage keepAliveMessage)
    {
        SendMessage(new KeepAliveServerMessage());

        if (_databaseModel == null!) return 1;
        _databaseModel.Document.PlayTimeSeconds += 30;
        _databaseModel.Document.LastActiveUnixTime = LogicTimeUtil.GetTimestamp();
        return 0;
    }

    // ReSharper disable once UnusedMember.Local
    private int LoginMessageReceived(LoginMessage loginMessage)
    {
        var languageManager = GlobalId.GetInstanceId(loginMessage.PreferredLanguage) switch
        {
            17 => LanguageManager.Russian,
            0 => LanguageManager.English,
            9 => LanguageManager.German,
            13 => LanguageManager.Chinese,
            _ => LanguageManager.English
        };

        /*if (!loginMessage.ClientVersion
                .Equals($"{Fingerprint.GetMajorField()}.{Fingerprint.GetBuildField()}"))
        {
            var loginFailedMessage = new LoginFailedMessage();
            {
                loginFailedMessage.ErrorCode = Convert.ToInt32(FailedCodeHelperTable.CustomMessage);
                loginFailedMessage.Reason =
                    languageManager.TranslateTo("Download the new client version, please!", out _);
            }

            SendMessage(loginFailedMessage);
            return 0;
        }*/

        var accountModel = new DatabaseModel<AccountDocument>();
        {
            if (loginMessage.AccountId < 1)
            {
                accountModel.SetDocument(new AccountDocument());
                DependencyHelperP1.GetAccountInject().AddOrReplace(accountModel);

                accountModel.GetDocument().AccountId = accountModel.Id + 1;
                accountModel.GetDocument().HomeId = HelperCity.GenerateRandomIntForBetween(1, 10);

                accountModel.GetDocument().PassToken = HelperCity.GenerateToken(accountModel.Id);

                accountModel.GetDocument().Device = loginMessage.Device;
                accountModel.GetDocument().StartDevice = loginMessage.Device;

                accountModel.GetDocument().StartPlayTimeSeconds = LogicTimeUtil.GetTimestamp();
                accountModel.GetDocument().AccountCreatedDate = $"{DateTime.Now:yyyy/MM/dd-(dddd) HH:mm:ss}";

                accountModel.GetDocument().PlayTimeSeconds = 10;
                accountModel.GetDocument().SessionsCount++;

                accountModel.GetDocument().UpgradeMaterial = 200;

                accountModel.GetDocument().HomeBrawlerGid =
                    GlobalId.CreateGlobalId(CsvFilesHelperTable.Characters.GetId(), 0);

                accountModel.GetDocument().ThumbnailDataGid =
                    GlobalId.CreateGlobalId(CsvFilesHelperTable.PlayerThumbnails.GetId(), 0);
                accountModel.GetDocument().NameColorDataGid =
                    GlobalId.CreateGlobalId(CsvFilesHelperTable.NameColors.GetId(), 0);

                /*accountModel.GetDocument().LogicOfferBundles = new Dictionary<int, LogicOfferBundle>
                {
                    {
                        0, new LogicOfferBundle([new LogicGemOffer(ShopItemHelperTable.MediumBox, 1)],
                            languageManager.TranslateTo("Hello new player!", out _),
                            ShopOfferBackgroundFormHelperTable.Bt21,
                            1, 0, 30, new TimeHelper(LogicTimeUtil.GetTimestamp()).AddDays(1), false, false)
                    }
                };*/

                /*accountModel.GetDocument().HeroEntries = [new HeroEntry(GlobalId.CreateGlobalId(16, 0))];
                accountModel.GetDocument().BrawlPassSeasonDats =
                    [new BrawlPassSeasonData(AppCustomizer.BrawlPassNowSeason)];

                accountModel.GetDocument().VanityItems = new VanityItems();
                accountModel.GetDocument().LogicQuests = new LogicQuests();*/

                accountModel.GetDocument().LanguageManager = languageManager;
                accountModel.GetDocument().IsOnline = true;

                goto label1;
            }

            accountModel = DependencyHelperP1.GetAccountInject().Get(loginMessage.AccountId - 1);
            if (accountModel == null)
            {
                var loginFailedMessage = new LoginFailedMessage();
                {
                    loginFailedMessage.ErrorCode = Convert.ToInt32(FailedCodeHelperTable.CustomMessage);
                    loginFailedMessage.Reason =
                        languageManager.TranslateTo("Clear the app data, please (DB error)!", out _);
                }

                SendMessage(loginFailedMessage);
                return -101;
            }

            if (!accountModel.GetDocument().PassToken.Equals(loginMessage.PassToken))
            {
                var loginFailedMessage = new LoginFailedMessage();
                {
                    loginFailedMessage.ErrorCode = Convert.ToInt32(FailedCodeHelperTable.CustomMessage);
                    loginFailedMessage.Reason =
                        languageManager.TranslateTo("Clear the app data, please (cipher error)!", out _);
                }

                SendMessage(loginFailedMessage);
                return -102;
            }

            accountModel.GetDocument().Device = loginMessage.Device;

            accountModel.GetDocument().PlayTimeSeconds += 10;
            accountModel.GetDocument().SessionsCount++;

            accountModel.GetDocument().LanguageManager = languageManager;
            accountModel.GetDocument().IsOnline = true;
        }

        label1:

        var loginOkMessage = new LoginOkMessage();
        {
            loginOkMessage.AccountId = accountModel.GetDocument().AccountId;
            loginOkMessage.HomeId = accountModel.GetDocument().HomeId;
            loginOkMessage.PassToken = accountModel.GetDocument().PassToken;
            loginOkMessage.FacebookId = "EE0";
            loginOkMessage.GamecenterId = "Z";
            loginOkMessage.ServerMajorVersion = 36;
            loginOkMessage.ContentVersion = 218;
            loginOkMessage.ServerBuild = 218;
            loginOkMessage.ServerEnvironment = "dev";
            loginOkMessage.SessionCount = accountModel.GetDocument().SessionsCount;
            loginOkMessage.PlayTimeSeconds = accountModel.GetDocument().PlayTimeSeconds;
            loginOkMessage.DaysSinceStartedPlaying =
                (LogicTimeUtil.GetTimestamp() - accountModel.GetDocument().StartPlayTimeSeconds) / 60 / 60 / 24;
            loginOkMessage.ServerTime = $"{DateTime.Now:yyyy/MM/dd-(dddd) HH:mm:ss}";
            loginOkMessage.AccountCreatedDate = accountModel.GetDocument().AccountCreatedDate;
            loginOkMessage.StartupCooldownSeconds = 60;
            loginOkMessage.LoginCountry = accountModel.GetDocument().LanguageManager.GetTextCodeByLanguage().ToUpper();
            loginOkMessage.KunlunId = "0f";
            loginOkMessage.Tier = 0;
            loginOkMessage.SecondsUntilAccountDeletion = 0;
        }

        var ownHomeDataMessage = new OwnHomeDataMessage();
        {
            ownHomeDataMessage.LogicClientHome = new LogicClientHome(accountModel);
            ownHomeDataMessage.LogicClientAvatar = new LogicClientAvatar(accountModel);
            ownHomeDataMessage.CurrentTimeInSecondsSinceEpoch = LogicTimeUtil.GetTimestamp();
        }

        SendMessage(loginOkMessage);
        SendMessage(ownHomeDataMessage);

        if (LogicHomeModeCollector.LogicHomeModes.TryGetValue(accountModel.Id, out var value))
        {
            value.AllocSession.Close();
            LogicHomeModeCollector.LogicHomeModes.TryRemove(accountModel.Id, out _);
        }

        LogicHomeModeCollector.LogicHomeModes.TryAdd(accountModel.Id, _logicHomeMode = new LogicHomeMode
        {
            AllocSession = AllocSession,
            LogicClientHome = ownHomeDataMessage.LogicClientHome,
            LogicClientAvatar = ownHomeDataMessage.LogicClientAvatar
        });

        _ownerMessageManager.SetAccountModel(accountModel.Id);

        if (!ActiveClientsRotator.AllocSessions.ContainsKey(accountModel.Id))
            ActiveClientsRotator.AllocSessions.TryAdd(accountModel.Id, []);

        ActiveClientsRotator.AllocSessions[accountModel.Id].Add(AllocSession);
        return 0;
    }
}
