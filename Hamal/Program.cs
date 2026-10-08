using Hamal.Model;
using Hamal.Servers;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("apssettings.json")
    .Build();

var servers = new HamalServers(configuration);



while (true)
{
    Parallel.Invoke(
                async () =>
                {

                    await servers.HmalTaskAsync("NORTH");
                },

                async () =>
                {
                    await servers.HmalTaskAsync("CENTER");
                },
                async () =>
                {
                    await servers.HmalTaskAsync("SOUTH");
                }, 
                async () =>
                {
                    await servers.HmalTaskAsync("OVERSEAS");
                });
}