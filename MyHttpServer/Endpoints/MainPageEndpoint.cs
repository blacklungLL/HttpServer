using System.Data.SqlClient;
using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Configurations;
using HttpServerLibrary.Core;
using HttpServerLibrary.HttpResponse;
using MyHttpServer.Models;
using MyORMLibrary;
using MyServer.services;
using TemplateEngine;

namespace MyHttpServer.Endpoints;

internal class MainEndpoint : EndpointBase
{
    [Get("Main")]
    public IHttpResponseResult GetMain()
    {
        // if (!IsAuthorized(Context)) // Используем метод проверки авторизации
        // {
        //     return Redirect("/auth/login");
        // }
        var localPath = "Templates/Pages/MainPage/index.html";
        var responseText = File.ReadAllText(localPath);
        var template = new HtmlTemplateEngine();

        var movieContext = new ORMContext<movies>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var movies = movieContext.ReadAll("movies");

        var countriesContext = new ORMContext<countries>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var countries = countriesContext.ReadAll("countries");

        var genresContext = new ORMContext<genres>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var genres = genresContext.ReadAll("genres");

        var popularContext = new ORMContext<popular>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var popular = popularContext.ReadAll("popular");
        
        var categoriesContext = new ORMContext<categories>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var categories = categoriesContext.ReadAll("categories");
        
        dynamic model = new
        {
            Movies = movies,
            Countries = countries,
            Genres = genres,
            Popular = popular,
            Categories = categories
        };
        var res = template.Render(responseText, model);
        return Html(res);
    }
        
    private bool IsAuthorized(HttpRequestContext context)
    {
        //Проверка наличия Cookie с session-token
        if (context.Request.Cookies.Any(c=> c.Name == "session-token"))
        {
            var cookie = context.Request.Cookies["session-token"];
            return SessionStorage.ValidateToken(cookie.Value);
        }
         
        return false;
    }
}