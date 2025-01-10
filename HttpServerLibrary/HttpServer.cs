using HttpServerLibrary.Handlers;
using System.Net;

namespace HttpServerLibrary
{
    public sealed class HttpServer
    {
        private readonly string _staticDirectoryPath;

        private readonly HttpListener _listener;

        private readonly Handler _staticFilesHandler;

        private readonly Handler _endpointsHandler;

        public HttpServer(string[] prefixes)
        {
            _listener = new HttpListener();
            foreach (string prefix in prefixes)
            {
                Console.WriteLine($"Prefixe: {prefix}");
                _listener.Prefixes.Add(prefix);
            }
            _staticFilesHandler = new StaticFilesHandler();
            _endpointsHandler = new EndpointsHandler();
        }

        public async Task Start()
        {
            _listener.Start();
            Console.WriteLine("Сервер запущен и ожидает запросов...");
            while (_listener.IsListening)
            {
                var context = _listener.GetContext();
                var requestContext = new HttpRequestContext(context.Request, context.Response);
                ProcessRequest(requestContext);
            }
        }

        private async Task ProcessRequest(HttpRequestContext context)
        {
            _staticFilesHandler.Successor = _endpointsHandler;
            _staticFilesHandler.HandleRequest(context);
        }
        
        public void Stop()
        {
            _listener.Stop();
            Console.WriteLine("Сервер остановлен.");
        }
    }
}
