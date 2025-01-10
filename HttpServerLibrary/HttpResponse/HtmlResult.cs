using System.Text;

namespace HttpServerLibrary.HttpResponse
{
    //создаем класс для ответа html
    internal class HtmlResult : IHttpResponseResult
    {
        //переменная в которой будет ответ
        private readonly string _html;
        
        // конструктор для ответа
        public HtmlResult(string html)
        {
            _html = html;
        }
        
        // метод для обработки и выполнения ответа, отправки html содержимого
        public void Execute(HttpRequestContext context)
        {
            
            //превращаем в массив байтов
            byte[] buffer = Encoding.UTF8.GetBytes(_html);

            // получаем поток ответа и пишем в него ответ
            context.Response.ContentLength64 = buffer.Length;
            using Stream output = context.Response.OutputStream;

            // отправляем данные
            output.Write(buffer);
            output.Flush();
        }
    }
}
