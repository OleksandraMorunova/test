using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace restful.UserAndTodoList.Model
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("username")]
        [JsonPropertyName("username")]
        [Required]
        public string? UserName { get; set; }

        [BsonElement("email")]
        [JsonPropertyName("email")]
        [EmailAddress(ErrorMessage = "Invalide email address")]
        [Required]
        public string? Email { get; set; }

        [BsonElement("password")]
        [JsonPropertyName("password")]
        [Required]
        public string? Password { get; set; }

        [BsonElement("datetime_creatin_user")]
        [JsonPropertyName("datetime_creatin_user")]
        public DateTime CreatedUserTime { get; set; }

        [BsonElement("date_of_birthday")]
        [JsonPropertyName("date_of_birthday")]
        public DateTime DateOfBirth { get; set; }

        [BsonElement("icon")]
        [JsonPropertyName("icon")]
        public ObjectId? LinkToProfileIcon { get; set; }

        [BsonElement("roles")]
        [JsonPropertyName("roles")]
        [Required]
        public string? Roles { get; set; }
    }
}
