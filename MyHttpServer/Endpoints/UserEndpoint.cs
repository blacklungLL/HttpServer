using System.Data.SqlClient;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Configurations;
using HttpServerLibrary.Core;
using HttpServerLibrary.HttpResponse;
using MyHttpServer.Models;
using MyORMLibrary;

namespace MyHttpServer.Endpoints;

public class UserEndpoints : EndpointBase
{
    [Get("user")]
    public IHttpResponseResult GetUserById(int id)
    {
        var context = new ORMContext<Users>(new SqlConnection(AppConfig.GetInstance().ConnectionString));

        var user = context.ReadById(id, "Users");
        return Json(user);
    }

    [Get("users")]
    public IHttpResponseResult GetAllUsers()
    {
        var context = new ORMContext<Users>(new SqlConnection(AppConfig.GetInstance().ConnectionString));

        var user = context.Where(u => u.Id == 3 || u.Id == 2);
        return Json(user);
    }

    [Get("deleteuser")]
    public IHttpResponseResult DeleteUser(int id)
    {
        var context = new ORMContext<Users>(new SqlConnection(AppConfig.GetInstance().ConnectionString));

        var user = context.Delete(id, "Users");
        return Json(user);
    }
}