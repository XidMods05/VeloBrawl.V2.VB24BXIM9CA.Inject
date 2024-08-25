using Microsoft.Extensions.DependencyInjection;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.DependencyLayer.P1.DI_P1;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.AttachmentProtocol;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.ServerCommands;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.Node_9.Server;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Server.Allocator.Alloc;

public class AllocSession : IAllocSession
{
    public INodeServerProducer NodeServerProducer { get; set; } = null!;
    public IOwnerMessageManager OwnerMessageManager { get; set; } = null!;

    public long DatabaseId { get; set; }
    public Guid SessionId { get; set; }

    public void SendMessage(IPiranhaMessage piranhaMessage)
    {
        OwnerMessageManager.SendMessage(piranhaMessage);
    }

    public void SendCommandWithoutExecutor(ILogicServerCommand logicServerCommand)
    {
        var availableServerCommandMessage =
            new AvailableServerCommandMessage(LogicHomeModeCollector.LogicHomeModes.GetValueOrDefault(DatabaseId)!);
        {
            availableServerCommandMessage.SetServerCommand(logicServerCommand);
        }

        SendMessage(availableServerCommandMessage);
    }

    public void SendDayChanged()
    {
        var availableServerCommandMessage =
            new AvailableServerCommandMessage(LogicHomeModeCollector.LogicHomeModes.GetValueOrDefault(DatabaseId)!);
        {
            availableServerCommandMessage.SetServerCommand(
                new LogicDayChangedCommand(new LogicConfData(DependencyHelperP1.GetAccountInject().Get(DatabaseId)!)));
        }

        SendMessage(availableServerCommandMessage);
    }

    public void Close(bool silentForCore = false, bool withBan = false)
    {
        if (withBan)
            DependencyManualInjector.ServiceProvider.GetRequiredService<IRabbitOut>()
                .SendMessage($"BAN_C1 {SessionId}");
        if (!silentForCore)
            DependencyManualInjector.ServiceProvider.GetRequiredService<IRabbitOut>().SendMessage($"CLOSE {SessionId}");

        OwnerMessageManager.Goodbye();
    }
}