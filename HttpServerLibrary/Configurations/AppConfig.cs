using System.Net.Mail;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpServerLibrary.Configurations
{
    public sealed class AppConfig
    {
        
        public const string FILE_NAME = "config.json";

        public static AppConfig _Instance;
        
        [JsonConstructor]
        public AppConfig()
        {
        }

        public static string Domain { get; set; } = "localhost";

        public static uint Port { get; set; } = 8888;

        public string Path { get; set; } = "public/";

        public string AddPath { get; set; } = "Templates/Pages/";

        public string ConnectionString { get; set; } =
            @"Data Source=localhost; Initial Catalog=PersonDB;User ID=sa;Password=HqR3dps76nmbT;";

        public SmtpClient? SmtpClient { get; set; }

        public NetworkCredential? NetworkCredential { get; set; }
        
        private void Initialize()
        {
            if (File.Exists(FILE_NAME))
            {
                var configFile = File.ReadAllText(FILE_NAME);
                _instance = JsonSerializer.Deserialize<AppConfig>(configFile);
            }
            else
            {
                Console.WriteLine($"Файл настроек {AppConfig.FILE_NAME} не найден");
            }
        }

        private static AppConfig _instance;
        
        public static AppConfig GetInstance()
        {
            if (_instance is null)
            {
                _instance = new AppConfig();
                _instance.Initialize();
            }

            return _instance;
        }
    }
}