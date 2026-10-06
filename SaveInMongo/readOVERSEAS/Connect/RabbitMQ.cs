using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharpCompress.Factories;
using System.Text;

namespace SaveToDB.connect;

public class RabitStrem
{
    private ConnectionFactory _factory;

    public RabitStrem(IConfiguration configuration)
    {
        var factory = new ConnectionFactory { HostName = configuration["RabbitMQ:HOSTNAME"]};
        
    }

    public async Task startRuning()
    {
        var connection = await _factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "hello", durable: true, exclusive: false, autoDelete: false,
        arguments: new Dictionary<string, object?> { { "x-queue-type", "quorum" } });

        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received {message}");
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync("hello", autoAck: true, consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    public async Task NORTHMessage()
    {
        var connection = await _factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "NORTH", durable: true, exclusive: false, autoDelete: false,
        arguments: new Dictionary<string, object?> { { "x-queue-type", "quorum" } });

        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received {message}");
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync("NORTH", autoAck: true, consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    public async Task CENTERMessage()
    {
        var connection = await _factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "CENTER", durable: true, exclusive: false, autoDelete: false,
        arguments: new Dictionary<string, object?> { { "x-queue-type", "quorum" } });

        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received {message}");
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync("CENTER", autoAck: true, consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    public async Task SOUTHMessage()
    {
        var connection = await _factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "SOUTH", durable: true, exclusive: false, autoDelete: false,
        arguments: new Dictionary<string, object?> { { "x-queue-type", "quorum" } });

        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received {message}");
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync("SOUTH", autoAck: true, consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    public async Task OVERSEASMessage()
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
            Console.WriteLine($" [x] Received {message}");
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync("OVERSEAS", autoAck: true, consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}
