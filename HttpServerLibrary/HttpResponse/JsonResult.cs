using System.Text;
using System.Text.Json;

namespace HttpServerLibrary.HttpResponse
{
    // класс для ответа в виде json c использованием наследования интерфейса
    internal class JsonResult : IHttpResponseResult
    {
        //переменная для данных
        private readonly object _data;
        
        // конструктор для инициализации данных на сериализацию
        public JsonResult(object data)
        {
            _data = data;
        }
        // метод выполнения ответа и отправки json-данных в тело запроса
        public void Execute(HttpRequestContext context)
        {
            // Сериализуем данные в JSON-строку
            var json = JsonSerializer.Serialize(_data);
        
            // Преобразуем JSON-строку в массив байто
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            
            //установка заголовка content-type
            context.Response.Headers.Add("Content-Type", "application/json");
            // получаем поток ответа и пишем в него ответ
            context.Response.ContentLength64 = buffer.Length;
            using Stream output = context.Response.OutputStream;

            // отправляем данные
            output.Write(buffer);
            output.Flush();
        }
    }
}