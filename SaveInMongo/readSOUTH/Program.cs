using Microsoft.Extensions.Configuration;
using readSOUTH.connect;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath (Directory.GetCurrentDirectory ())
    .AddJsonFile("apssettings.json")
    .Build ();

var mongo = new MyMongoConnect(configuration);

var rabbit = new RabbitStrem(configuration, mongo);

while (true)
{
    try:

}
