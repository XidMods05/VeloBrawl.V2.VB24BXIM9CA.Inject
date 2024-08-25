using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Reflection;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.DependencyLayer.P1.DI_P1;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.Database.Newbie;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.AttachmentProtocol;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.DatabaseProtocol;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.ServerCommands;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_9.Client;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_9.Server;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.HelpDirectory;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Server.Node_9.Laser;

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
        _logicHomeMode = LogicHomeModeCollector.LogicHomeModes.GetValueOrDefault(databaseId)!;
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
        //throw new NotImplementedException();
    }

    // ReSharper disable once UnusedMember.Local
    private int EndClientTurnMessageReceived(EndClientTurnMessage endClientTurnMessage)
    {
        return _logicHomeMode.EndClientTurnReceived(endClientTurnMessage.Tick, endClientTurnMessage.Checksum,
            endClientTurnMessage.Commands);
    }

    // ReSharper disable once UnusedMember.Local
    private int ChangeAvatarNameMessageReceived(ChangeAvatarNameMessage changeAvatarNameMessage)
    {
        if (changeAvatarNameMessage.NameSetByUser) return -1;

        if (HelperCity.GetIsAdequateString(changeAvatarNameMessage.Name) &&
            HelperCity.GetIsCorrectName(changeAvatarNameMessage.Name))
        {
            var availableServerCommandMessage = new AvailableServerCommandMessage(_logicHomeMode);
            {
                availableServerCommandMessage
                    .SetServerCommand(
                        new LogicChangeAvatarNameCommand(changeAvatarNameMessage.Name, 0));
            }

            SendMessage(availableServerCommandMessage);
            return 0;
        }

        var avatarNameChangeFailed = new AvatarNameChangeFailedMessage();
        {
            avatarNameChangeFailed.Reason =
                changeAvatarNameMessage.Name.Length < 3 ? 2 : changeAvatarNameMessage.Name.Length >= 15 ? 1 : 0;
        }

        SendMessage(avatarNameChangeFailed);
        return 0;
    }
}