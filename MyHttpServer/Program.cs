using HttpServerLibrary.Configurations;
using HttpServerLibrary;

namespace MyHttpServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            AppConfig config = AppConfig.GetInstance();
            
            var prefixes = new[] { $"http://{AppConfig.Domain}:{AppConfig.Port}/" };
            var server = new HttpServer(prefixes);

            await server.Start();
        }
    }
}
