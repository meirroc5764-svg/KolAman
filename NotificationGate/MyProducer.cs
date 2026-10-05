using Confluent.Kafka;

namespace NotificationGate.Producer;

public class MyKafkaProducer
{
    private IProducer<Null, string> _producer;
    public MyKafkaProducer(string server)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = server
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public void SendMessage(string message, string topic)
    {
        try
        {
            var resultMessage = new Message<Null, string>
            {
                Value = message
            };
            _producer.Produce(topic, resultMessage);
            Console.WriteLine(resultMessage.ToString());
        }
        catch (Exception ex)
        {
           Console.WriteLine(ex.ToString());
        }
    }
}