namespace HttpServerLibrary.Attributes
{
    //определение аттрибута getattribute который можно применять только к методам
    [AttributeUsage(AttributeTargets.Method)]
    public class GetAttribute : Attribute
    {
        // заводим переменную пути к get запроса
        public string Route { get; }
        
        // конструктор для пути
        public GetAttribute(string route)
        {
            Route = route;
        }
    }
}
