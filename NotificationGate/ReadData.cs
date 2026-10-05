using NotificationGate.Producer;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace NotificationGate.ReadFile;

public class ReadFile
{
    public string ReadData(string filename)
    {
        var producer = new MyKafkaProducer("localhost:9092");
        if (_CheckReady(filename))
        {
            var data = File.ReadAllText($"{filename}\\alert.json");

            JsonDocument json = JsonDocument.Parse(data);

            producer.SendMessage(json.ToString(), "test-topic1");

        return json.ToString();
        }
        return null;
    }

    private bool _CheckReady(string fullname)
    {
        if (File.Exists($"{fullname}\\alert.ready"))
        {
            return false;
        }
        return true;
    }
}