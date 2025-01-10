using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Core;
using HttpServerLibrary.HttpResponse;
using MyHttpServer.Models;

namespace MyHttpServer.Endpoints
{
    // [Endpoint("route")]
    internal class SendEmailEndpoint : EndpointBase
    {
        
        [Get("anime")]
        public IHttpResponseResult GetAnimePage()
        {
            // логика отправки страницы Аниме с формой
            Console.WriteLine("Ура anime");

            var user = new { Name = "Иван", Email = "test@test.ru"};

            return Json(user);
        }

        [Get("wow")]
        public IHttpResponseResult GetLoginPage(string hello)
        {
            // логика отправки страницы Логин с формой
            Console.WriteLine(hello);

            string responseText = "Ура " + hello;

            return Html(responseText);
        }

        [Get("home-work")]
        public void GetHomeWorkPage()
        {
            // логика
        }

        [Post("anime")]
        public void SendEmailAnime()
        {
            // логика отправки сообщения про Аниме
        }

        [Post("login")]
        public void SendEmailLogin(string login, string password)
        {
            // логика отправки сообщения что вы вошли в систему, дата и время

        }

        [Post("home-work")]
        public void SendEmailHomeWork()
        {
            // логика отправки ДЗ на {дата}
        }

    }
}