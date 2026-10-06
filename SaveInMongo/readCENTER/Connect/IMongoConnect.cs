namespace readCENTER.connect;

public interface IMongoConnect
{
    public void SendToMongo(string NameCollection, string message);
}