using Microsoft.Extensions.Configuration;
using NotificationGate.Producer;
using Serilog;
using System;
using System.IO;
using System.Text.Json;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("apssetings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(configuration["Elasicsearch:server"])
    .CreateLogger();



namespace MyNamespace
{
    class MyClassCS
    {

        static void Main()
        {
            using var watcher = new FileSystemWatcher(@"C:\Users\Aenigma\Downloads\alert-simulator\alert-simulator\alerts\aman");

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;


            watcher.Created += OnCreated;

            watcher.Filter = "*.ready";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            watcher.InternalBufferSize = 65536;

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }


        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            var producer = new MyKafkaProducer("localhost:9092");
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);

            var path = (e.FullPath.ToString().Replace(".ready", ""));

            var pathResult = (path + ".json");

            var data = File.ReadAllText($"{pathResult}");

            Console.WriteLine(data);


            producer.SendMessage(data, "first-topic");


        }
    }
}