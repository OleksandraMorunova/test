using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
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
        public string? UserName { get; set; }

        [BsonElement("email")]
        [JsonPropertyName("email")]
        [EmailAddress(ErrorMessage = "Invalide email address")]
        public string? Email { get; set; }

        [BsonElement("password")]
        [JsonPropertyName("password")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
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
        public string? Roles { get; set; }
    }
}
