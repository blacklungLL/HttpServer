using System.Net;

namespace HttpServerLibrary.HttpResponse
{
    
    // соответственно создаем интерфейс для ответа
    public interface IHttpResponseResult
    {
        //метод для отправки ответа и отправки данных
        void Execute(HttpRequestContext context);
    }
}
