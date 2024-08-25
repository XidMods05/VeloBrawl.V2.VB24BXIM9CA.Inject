using System.Collections.Frozen;
using System.Reflection;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages;

public static class LogicLaserMessageFactory
{
    private static FrozenDictionary<int, Type> _messages;

    static LogicLaserMessageFactory()
    {
        _messages = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(PiranhaMessage)))
            .Where(t => t.GetConstructors().Any(c => c.GetParameters().Length == 0))
            .ToDictionary(
                messageTypeof => (Activator.CreateInstance(messageTypeof) as PiranhaMessage)!.GetMessageType())
            .ToFrozenDictionary();
    }

    public static PiranhaMessage? CreateMessageByType(int type)
    {
        if (_messages.TryGetValue(type, out var message))
            return Activator.CreateInstance(message) as PiranhaMessage;
        return null;
    }

    public static void Destruct()
    {
        _messages = null!;
    }
}