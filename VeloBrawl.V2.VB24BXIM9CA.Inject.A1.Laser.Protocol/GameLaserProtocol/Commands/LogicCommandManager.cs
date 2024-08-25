using System.Collections.Frozen;
using System.Reflection;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Auxiliary.Debug;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands.Collection.AutoMatch.Own;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Commands;

public static class LogicCommandManager
{
    private static FrozenDictionary<int, Type> _commands;

    static LogicCommandManager()
    {
        _commands = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(LogicCommand)))
            .Where(t => t.GetConstructors().Any(c => c.GetParameters().Length == 0))
            .ToDictionary(commandTypeof => (Activator.CreateInstance(commandTypeof) as LogicCommand)!.GetCommandType())
            .ToFrozenDictionary();
    }

    public static LogicCommand? DecodeCommand(ByteStream byteStream)
    {
        var logicCommand = CreateCommand(byteStream.ReadVInt());
        if (logicCommand == null) return null;

        logicCommand.Decode(byteStream);
        return logicCommand;
    }

    public static void EncodeCommand(LogicCommand logicCommand, ByteStream byteStream, LogicHomeMode? logicHomeMode)
    {
        byteStream.WriteVInt(logicCommand.GetCommandType());
        logicCommand.Encode(byteStream);

        if (logicHomeMode == null) return;
        if (!logicCommand.CanExecute(logicHomeMode)) return;

        logicCommand.Execute(logicHomeMode);
        ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Lobby,
            $"New command executed! Info: Type = {logicCommand.GetCommandType()}; Name = {DebugInfoCollector.CommandCollectorY.GetValueOrDefault(logicCommand.GetCommandType(), "unknown-name")}.");
    }

    public static void EncodeCommand(ILogicServerCommand logicCommand, ByteStream byteStream)
    {
        byteStream.WriteVInt(logicCommand.GetCommandType());
        logicCommand.Encode(byteStream);

        ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Lobby,
            $"New command encoded! Info: Type = {logicCommand.GetCommandType()}; Name = {DebugInfoCollector.CommandCollectorY.GetValueOrDefault(logicCommand.GetCommandType(), "unknown-name")}.");
    }

    public static LogicCommand? CreateCommand(int type)
    {
        if (_commands.TryGetValue(type, out var command))
            return Activator.CreateInstance(command) as LogicCommand;

        ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Lobby,
            $"New unknown command handled! Info: Type = {type}; Name = {DebugInfoCollector.CommandCollectorY.GetValueOrDefault(type, "unknown-name")}.");
        return null;
    }

    public static void Destruct()
    {
        _commands = null!;
    }
}