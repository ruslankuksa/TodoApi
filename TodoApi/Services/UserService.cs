using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoApi.Settings;
using TodoApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace TodoApi.Services;

public class UserService
{
	private readonly IMongoCollection<User> users;
	private readonly string key;

	public UserService(IConfiguration configuration)
	{
		var client = new MongoClient(configuration.GetSection("Database")["ConnectionString"]);

		var mongoDatabase = client.GetDatabase("TodoStore");

		users = mongoDatabase.GetCollection<User>("Users");

		key = configuration.GetSection("JWT").ToString();
	}

	public async Task<UserDTO?> Authenticate(string email, string password)
    {
		var user = await users.Find(usr => usr.Email == email && usr.Password == password).FirstOrDefaultAsync();

		if (user == null) {
			return null;
        }

		var dtoUser = new UserDTO(user);

		return dtoUser;
	}
		

	public async Task Create(User newUser)
    {
		await users.InsertOneAsync(newUser);
	}
		

	public string generateJWToken(string email)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenKey = Encoding.ASCII.GetBytes(key);
		var tokenDescriptor = new SecurityTokenDescriptor()
		{
			Subject = new ClaimsIdentity(new Claim[] {
				new Claim(ClaimTypes.Email, email),
			}),

			Expires = DateTime.UtcNow.AddDays(7),

			SigningCredentials = new SigningCredentials(
				new SymmetricSecurityKey(tokenKey),
				SecurityAlgorithms.HmacSha256Signature
			)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}
}

