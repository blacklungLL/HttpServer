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

public class FilmEndpoint : EndpointBase
{
    [Get("film")]
    public IHttpResponseResult GetFilm(int id)
    {
        
        if (!IsAuthorized(Context))
        {
            return Redirect("auth/login");
        }
        var localPath = "Templates/Pages/DeadpoolFilm/Deadpool.html";
        var responseText = File.ReadAllText(localPath);

        var movieContext = new ORMContext<MovieData>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var movie = movieContext.ReadMovieById(id);
        
        var template = new HtmlTemplateEngine();
        var res = template.Render(responseText, movie);
        return Html(res);
    }
    
    
    private bool IsAuthorized(HttpRequestContext context)
    {
        // Проверка наличия Cookie с session-token
        if (context.Request.Cookies.Any(c=> c.Name == "session-token"))
        {
            var cookie = context.Request.Cookies["session-token"];
            return SessionStorage.ValidateToken(cookie.Value);
        }
         
        return false;
    }
}