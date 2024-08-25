namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;

public interface IMessageManager
{
    public IAllocSession AllocSession { get; set; }

    public void SetAccountModel(long databaseId);

    public int ReceiveMessage(IPiranhaMessage piranhaMessage);
    public int SendMessage(IPiranhaMessage piranhaMessage);
    public void Goodbye();
}