namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;

public interface IRabbitMqMessageProcessor
{
    public void ConsumerCallback(byte[] message);
    public void AddProducer(INodeServerProducer nodeServerProducer);
}