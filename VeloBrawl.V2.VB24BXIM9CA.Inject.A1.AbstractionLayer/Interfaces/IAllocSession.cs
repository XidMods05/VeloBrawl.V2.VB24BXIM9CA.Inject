namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;

public interface IAllocSession
{
    public IOwnerMessageManager OwnerMessageManager { get; set; }

    public long DatabaseId { get; set; }
    public Guid SessionId { get; set; }

    public void SendMessage(IPiranhaMessage piranhaMessage);
    public void SendCommandWithoutExecutor(ILogicServerCommand logicServerCommand);

    public void SendDayChanged();

    public void Close(bool silentForCore = false, bool withBan = false);
}