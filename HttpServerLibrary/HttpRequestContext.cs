using System.Net;

namespace HttpServerLibrary
{
    // класс для предоставления контекста http запроса
    public sealed class HttpRequestContext
    {
        // сам запрос
        public HttpListenerRequest Request { get; }
        
        // ответ
        public HttpListenerResponse Response { get; }
        
        // конструктор, инициализирующий запрос и ответ
        public HttpRequestContext(HttpListenerRequest request, HttpListenerResponse response)
        {
            //запрос
            Request = request;
            //ответ
            Response = response;
        }
    }
}
