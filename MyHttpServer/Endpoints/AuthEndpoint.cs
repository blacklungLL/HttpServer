using System.Data.SqlClient;
using System.Net;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Configurations;
using HttpServerLibrary.Core;
using HttpServerLibrary.HttpResponse;
using MyHttpServer.Models;
using MyORMLibrary;
using MyServer.services;

namespace MyHttpServer.Endpoints;

public class AuthEndpoint : EndpointBase
{
    [Get("auth/login")]
    public IHttpResponseResult Get()
    {
        var loginPath = "Templates/Pages/Auth/signin.html";
        var responseText = File.ReadAllText(loginPath);
        return Html(responseText);
    }
    
    [Post("auth/login")]
    public IHttpResponseResult Login(string login, string password)
    {
        var context = new ORMContext<Users>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var user = context.FirstOrDefault(u => u.Login == login && u.Password == password);
        
        if(user == null)
        {
            return Redirect("/auth/login");
        }
        
        var token = Guid.NewGuid().ToString();
        Cookie nameCookie = new Cookie("session-token", token);
        nameCookie.Path = "/";

        Context.Response.Cookies.Add(nameCookie);
        SessionStorage.SaveSession(token, password.ToString());
            
        Console.WriteLine($"Login: {login}, Password: {password}");
        return Redirect("/main");
    }
}