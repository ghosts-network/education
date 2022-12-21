using GhostNetwork.Gateway.Education.MongoDb;
using MongoDB.Driver;

namespace GhostNetwork.Education.Api.Integrations;

public class MongoDbContext
{
    private readonly IMongoDatabase database;

    public MongoDbContext(IMongoDatabase database)
    {
        this.database = database;
    }

    public IMongoCollection<FlashCardTestHistoryEntity> History =>
        database.GetCollection<FlashCardTestHistoryEntity>("fsHistory");

    public IMongoCollection<FlashCardCurrentProgressEntity> Progress =>
        database.GetCollection<FlashCardCurrentProgressEntity>("fsProgress");
}