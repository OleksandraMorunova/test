using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization;

namespace restful.UserAndTodoList.Model
{
    public class TodoListModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonId]
        [BsonElement("user_id")]
        [JsonPropertyName("user_id")]
        public ObjectId? UserId { get; set; }

        [BsonElement("title")]
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [BsonElement("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
