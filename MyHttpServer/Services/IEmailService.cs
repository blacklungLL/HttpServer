using HttpServerLibrary.Configurations;


namespace MyHTTPServer.services
{
    public interface IEmailService
    {
        void SendEmail(string email, string title, string message, AppConfig config);
    }
}