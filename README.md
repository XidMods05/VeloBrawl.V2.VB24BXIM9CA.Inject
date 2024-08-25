# VeloBrawl.V2.VB24BXIM9CA.Inject
This is an injection for VeloBrawl V2 that uses a system broker on RabbitMQ and stores data using PostgreSQL or MongoDB (your choice). 

# To run it you need:
## Dotnet v8.0.401.
## RabbitMQ v3.13.6.
## PostgreSQL or MongoDB.

# You can use [VeloBrawl.V2.VB24BXIM9CA.Core](https://github.com/XidMods05/VeloBrawl.V2.VB24BXIM9CA.Core) as program core.

You can configure your application in the VeloBrawl.V2.VB24BXIM9CA.Inject.A1/SaveBase/config.json file.

logSensitive - the level of how sensitive the console is to logging 

public enum UniqueLogLevels : byte
{
    Hypersensitive = 100,
    MediumSensitive = 80,
    AlmostSensitive = 40,
    AlmostInsensitive = 30,
    Insensitive = 20,
    FatalInsensitive = 10,
    CrazyNoneLogLevel = 1,
    LogAbsent = 0
},

rabbitMqHost - host for brokerage with RabbitMQ.
timeInSecsToNextEventUpdate - time for the next event update.
databaseName - the database you want to use as data storage. (mongodb / postgres).

mongoDbConnectionString / mongoDbClusterName - information for MongoDB.
postgresConnectionString - link to connect to Postgres.

minutesToGC - number of minutes after which garbage collection will be performed. (minimum number of minutes is 1).