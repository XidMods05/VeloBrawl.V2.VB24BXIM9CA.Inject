using Newtonsoft.Json;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Butler.SharedProtocol;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.TablesOfDataSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.TablesOfDataSector.Manufacturer.LaserMicro;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.Enumerations.Extension;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.Enumerations.Game;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.HelpDirectory;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.InGameUtilities;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.InGameUtilities.Equipment.NotStatic;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging;
using static VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Butler.LobbyRotator.VisibilityRotator;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Butler.LobbyRotator;

public static class VisibilityRotatorAdministrator
{
    public static void InitializeRotateEvents(string path)
    {
        FilePath = path;
        Event1DataMassive = new Dictionary<int, EventData>();
        Event2DataMassive = new Dictionary<int, EventData>();

        var json = File.ReadAllText(FilePath);
        dynamic data = JsonConvert.DeserializeObject(json)!;

        foreach (var slot in data.slots)
        {
            int slotNumber = slot.slot;
            string gameMode = slot.game_modes[0];

            var dataTable = LogicDataTables.GetAllDataFromCsvById(CsvFilesHelperTable.Locations.GetId());
            {
                new Random().Shuffle(dataTable);

                if (dataTable.Cast<LogicLocationData?>()
                        .Count(location => location!.GetGameModeVariation() == gameMode) <= 1)
                {
                    ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Warn,
                        $"Unknown game mode in event slot. Information: es: {slotNumber}; gmv: {gameMode}. Game mode redesigned into 'GemGrab'.");
                    gameMode = "GemGrab";
                }

                if (dataTable.Cast<LogicLocationData?>().Where(location => location!.GetGameModeVariation() == gameMode)
                        .Count(location => location!.GetDisabled()) >= dataTable
                        .Cast<LogicLocationData?>().Count(location => location!.GetGameModeVariation() == gameMode) - 1)
                {
                    ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Warn,
                        $"A large number of disabled locations were found and therefore it is impossible to start the location selection. Information: es: {slotNumber}; gmv: {gameMode}. Game mode redesigned into 'GemGrab'.");
                    gameMode = "GemGrab";
                }

                var locationS = false;
                while (!locationS)
                    foreach (var locateData in dataTable)
                    {
                        if (locationS) break;

                        var locate = (LogicLocationData)locateData;

                        if (!HelperCity.GetChanceByPercentage(35) ||
                            locate.GetGameModeVariation() != gameMode ||
                            locate.GetDisabled()) continue;

                        Event1DataMassive.Add(slotNumber - 1, new EventData(
                            new TimeHelper(LogicTimeUtil.GetTimestamp()).AddSeconds(
                                (int)DefaultSecTimeForEventUpdate),
                            slotNumber, locate.GetInstanceId(), 0));
                        Event2DataMassive.Add(slotNumber - 1, new EventData(
                            new TimeHelper(LogicTimeUtil.GetTimestamp()).AddSeconds(
                                (int)DefaultSecTimeForEventUpdate),
                            slotNumber, locate.GetInstanceId(), 0, true));

                        locationS = true;
                    }
            }
        }

        EventRotator();

        ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Start,
            $"VisibilityRotator started! Information: default time to next update event: {DefaultSecTimeForEventUpdate} (sec).");
    }

    private static void EventRotator()
    {
        new Thread(() =>
        {
            while (true)
            {
                Thread.Sleep((int)DefaultSecTimeForEventUpdate * 1000);

                var json = File.ReadAllText(FilePath);
                dynamic data = JsonConvert.DeserializeObject(json)!;

                foreach (var slot in data.slots)
                {
                    int slotNumber = slot.slot;
                    var deltaSector = new Random().Next(0, slot.game_modes.Count - 1);
                    string gameMode = slot.game_modes[deltaSector];

                    var dataTable =
                        LogicDataTables.GetAllDataFromCsvById<LogicLocationData>(CsvFilesHelperTable.Locations.GetId());
                    {
                        new Random().Shuffle(dataTable);

                        var mode = gameMode;
                        if (dataTable.Count(location => location.GetGameModeVariation() == mode) <= 1)
                        {
                            ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Warn,
                                $"Unknown game mode in event slot. Information: es: {slotNumber}; gmv: {gameMode}; gms: {deltaSector}. Game mode redesigned into 'GemGrab'.");
                            gameMode = "GemGrab";
                        }

                        var mode1 = gameMode;
                        if (dataTable.Where(location => location.GetGameModeVariation() == gameMode)
                                .Count(location => location.GetDisabled()) >= dataTable
                                .Cast<LogicLocationData?>()
                                .Count(location => location!.GetGameModeVariation() == mode1) - 1)
                        {
                            ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Warn,
                                $"A large number of disabled locations were found and therefore it is impossible to start the location selection. Information: es: {slotNumber}; gmv: {gameMode}; gms: {deltaSector}. Game mode redesigned into 'GemGrab'.");
                            gameMode = "GemGrab";
                        }

                        var locationS = false;
                        while (!locationS)
                        {
                            new Random().Shuffle(dataTable);

                            foreach (var locateData in dataTable)
                            {
                                if (locationS) break;

                                if (!HelperCity.GetChanceByPercentage(35) ||
                                    locateData.GetGameModeVariation() != gameMode ||
                                    locateData.GetDisabled()) continue;

                                Event1DataMassive[slotNumber - 1] = new EventData(
                                    new TimeHelper(LogicTimeUtil.GetTimestamp()).AddSeconds(
                                        (int)DefaultSecTimeForEventUpdate),
                                    slotNumber, locateData.GetInstanceId(), 0);

                                Event2DataMassive[slotNumber - 1] = new EventData(
                                    new TimeHelper(LogicTimeUtil.GetTimestamp()).AddSeconds(
                                        (int)DefaultSecTimeForEventUpdate),
                                    slotNumber, locateData.GetInstanceId(), 0, true);

                                locationS = true;
                            }
                        }
                    }
                }

                try
                {
                    Parallel.ForEach(ActiveClientsRotator.AllocSessions, session =>
                    {
                        foreach (var ass in session.Value)
                            ass.SendDayChanged();
                    });
                }
                catch (Exception e)
                {
                    ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Error, e);
                }
            }
        }).Start();
    }
}