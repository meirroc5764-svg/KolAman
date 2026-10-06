using MongoDB.Bson.Serialization.Attributes;

namespace readSOUTH.Model;

public class WarnningMessage
{
    [BsonElement("alert_id")]
    public string Alert_id { get; set; } = string.Empty;

    [BsonElement("source")]
    public string Source { get; set; } = string.Empty;

    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    [BsonElement("priority")]
    public string Priority { get; set; } = string.Empty;

    [BsonElement("classification")]
    public string Classification { get; set; } = string.Empty;

    [BsonElement("lat")]
    public double Lat { get; set; }

    [BsonElement("lon")]
    public double Lon { get; set; }

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.Now; 

    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;
}