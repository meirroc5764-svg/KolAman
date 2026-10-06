using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using readSOUTH.Model;
using System.Text.Json;

namespace readSOUTH.connect;

public class MyMongoConnect : IMongoConnect
{
    private IMongoDatabase _database;
    public MyMongoConnect(IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable(configuration["Mongodb:localhost:27017"]);
        if (connectionString == null)
        {
            Console.WriteLine("You must set your 'MONGODB_URI' environment variable. To learn how to set it, see https://www.mongodb.com/docs/drivers/csharp/current/get-started/create-connection-string");
            Environment.Exit(0);
        }
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("hazard-warning");
    }

    public void SendToMongo(string NameCollection, string message)
    {
        var collection = _database.GetCollection<WarnningMessage>(NameCollection);

        var json = JsonSerializer.Deserialize<WarnningMessage>(message);

        collection.InsertOne(json);
    }
}