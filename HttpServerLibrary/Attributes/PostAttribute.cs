namespace HttpServerLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PostAttribute : Attribute
    {
        // заводим переменную пути для пост запроса
        public string Route { get; }
        
        // конструктор
        public PostAttribute(string route)
        {
            Route = route;
        }
    }
}
