using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Models;

public class ProjectEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ChartEntity> Charts { get; set; } = [];
}