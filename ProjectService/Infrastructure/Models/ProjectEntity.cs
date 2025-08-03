using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectService.Infrastructure.Models
{
    /// <summary>
    /// Persistence model for a project stored in MongoDB. Contains BSON-specific attributes.
    /// </summary>
    public class ProjectEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public IList<ChartEntity> Charts { get; set; } = new List<ChartEntity>();
    }

    public class ChartEntity
    {
        public string Symbol { get; set; } = string.Empty;
        public string Timeframe { get; set; } = string.Empty;
        public IList<IndicatorEntity> Indicators { get; set; } = new List<IndicatorEntity>();
    }

    public class IndicatorEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Parameters { get; set; } = string.Empty;
    }

    /// <summary>
    /// Persistence model for user settings stored in MongoDB.
    /// </summary>
    public class UserSettingsEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public int UserId { get; set; }
        public string Language { get; set; } = "English";
        public string Theme { get; set; } = "light";
    }
}