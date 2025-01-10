using System.Net;

namespace HttpServerLibrary.Handlers
{
    //создание абстрактного класса handler
    abstract class Handler
    {
        // следующий обработчик в цепи
        public Handler Successor { get; set; }
        // абстрактный метод для обработки http запроса
        public abstract void HandleRequest(HttpRequestContext context);
    }

}
