using Hamal.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Hamal.Servers;

public class HamalServers
{
    private IMongoDatabase _dataBase;

    public HamalServers(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["MongoDb:server"]);
        _dataBase = client.GetDatabase(configuration["MongoDb:database"]);
    }

    public async Task HmalTaskAsync(string collectionName)
    {
        var LOW = 100;
        var MEDIUM = 200;
        var HIGH = 300;
        var CRITICAL = 400;


        var collection = _dataBase.GetCollection<WarnningMessage>(collectionName);

        var results = collection.Find(_ => true).ToList();

        foreach (var result in results)
        {
            var wait = result.Priority;

            if (wait == "LOW")
            {
                await Task.Delay(LOW);
            }

            if (wait == "MEDIUM")
            {
                await Task.Delay(MEDIUM);
            }

            if (wait == "HIGH")
            {
                await Task.Delay(HIGH);
            }

            if (wait == "CRITICAL")
            {
                await Task.Delay(CRITICAL);
            }
        }
    }
}