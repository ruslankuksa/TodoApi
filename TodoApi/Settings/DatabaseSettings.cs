using TodoApi.Models;

namespace TodoApi.Settings
{
	public class DatabaseSettings
	{
		public string ConnectionString { get; set; } = null!;

		public string DatabaseName { get; set; } = null!;
	}
}

