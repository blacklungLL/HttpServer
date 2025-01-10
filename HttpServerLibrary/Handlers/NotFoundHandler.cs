using HttpServerLibrary.HttpResponse;
using System.Text;

namespace HttpServerLibrary.Handlers
{
    // Класс для обработки запросов, которые не были найдены
    internal sealed class NotFoundHandler : Handler
    {
        // Переопределение метода HandleRequest для обработки запросов
        public override void HandleRequest(HttpRequestContext context)
        {
            // Создание объекта NotFoundResponse и выполнение его метода Execute
            var response = new NotFoundResponse();
            response.Execute(context);
        }
    }

    // Класс для представления ответа "Not Found"
    internal class NotFoundResponse : IHttpResponseResult
    {
        // Метод для выполнения ответа
        public void Execute(HttpRequestContext context)
        {
            // Установка статуса ответа на 404
            context.Response.StatusCode = 404;
            // Установка описания статуса
            context.Response.StatusDescription = "Not Found";
            // Установка типа контента
            context.Response.ContentType = "text/plain";
            // Создание буфера с сообщением "404 Not Found"
            byte[] buffer = Encoding.UTF8.GetBytes("404 Not Found");
            // Запись буфера в поток ответа
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
        }
    }
}