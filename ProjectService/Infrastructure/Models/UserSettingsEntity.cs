using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Models;

public class UserSettingsEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public int UserId { get; set; }
    public string Language { get; set; } = "English";
    public string Theme { get; set; } = "light";
}