using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace TodoApi.Models
{
	public class UserDTO
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("firstName")]
		public string FirstName { get; set; }

		[JsonPropertyName("lastName")]
		public string LastName { get; set; }

		[JsonPropertyName("email")]
		public string Email { get; set; }

		[JsonPropertyName("token")]
		public string Token { get; set; }

		public UserDTO(User user)
        {
			this.Id = user.Id;
			this.FirstName = user.FirstName;
			this.LastName = user.LastName;
			this.Email = user.Email;
        }

		public void setToken(string token)
        {
			this.Token = token;
        }
	}
}

