using MongoDB.Bson.Serialization.Attributes;

namespace GhostNetwork.Gateway.Education.MongoDb;

public class FlashCardTestHistoryAnswerEntity
{
    [BsonElement("cardId")]
    public string CardId { get; set; } = null!;

    [BsonElement("answer")]
    public string Answer { get; set; } = null!;
}