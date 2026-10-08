using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharpCompress.Factories;
using System.Text;

namespace readOVERSEAS.connect;

public class RabbitStrem
{
    public IMongoConnect _mongo;
    private ConnectionFactory _factory;

    public RabbitStrem(IConfiguration configuration, IMongoConnect mongo)
    {
        _mongo = mongo;

        _factory = new ConnectionFactory { HostName = configuration["RabbitMQ:HOSTNAME"] };
    }

    public async Task startRuning()
    {
        var connection = await _factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "OVERSEAS", durable: true, exclusive: false, autoDelete: false,
        arguments: new Dictionary<string, object?> { { "x-queue-type", "quorum" } });

        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            if (message != null)
            {
                _mongo.SendToMongo("OVERSEAS", message);

                Console.WriteLine($" [x] Received and send {message}");
            }

            Console.WriteLine("message null");

            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync("OVERSEAS", autoAck: true, consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}