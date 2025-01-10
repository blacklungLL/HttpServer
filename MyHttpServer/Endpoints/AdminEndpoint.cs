using System.Data.SqlClient;
using System.Net;
using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Configurations;
using HttpServerLibrary.Core;
using HttpServerLibrary.HttpResponse;
using MyHttpServer.Helpers;
using MyHttpServer.Models;
using MyORMLibrary;
using MyServer.services;
using TemplateEngine;

namespace MyHttpServer.Endpoints;

public class AdminEndpoint : EndpointBase
{
    public virtual IResponseHelper ResponseHelper { get; set; } = new ResponseHelper();
    
    [Get("admlog")]
    public IHttpResponseResult Get()
    {
        var loginPath = "Templates/Pages/AdminAuth/adminLogin.html";
        var responseText = File.ReadAllText(loginPath);
        Console.WriteLine("прочитано");
        return Html(responseText);
    }

    [Post("admlog")]
    public IHttpResponseResult Login(string login, string password)
    {
        var context = new ORMContext<Admins>(new SqlConnection(AppConfig.GetInstance().ConnectionString));

        var id = int.Parse(password);
        var admin = context.FirstOrDefault(u => u.email == login && u.id == id);

        if (admin == null)
        {
            return Redirect("/admlog");
        }

        var token = Guid.NewGuid().ToString();
        Cookie nameCookie = new Cookie("admin-session-token", token);
        nameCookie.Path = "/";

        Context.Response.Cookies.Add(nameCookie);
        SessionStorage.SaveSession(token, password);

        Console.WriteLine($"Login: {login}, Password: {password}");
        return Redirect("/admin");
    }
    
    [Get("admin")]
    public IHttpResponseResult GetAdmin()
    {
        var localPath = "Admin/admin.html";
        var responseText = ResponseHelper.GetResponseText(localPath);
        var template = new HtmlTemplateEngine();
        
        Console.WriteLine("----- admin on admin-page -----");
        
        var movieContext = new ORMContext<movies>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var movies = movieContext.ReadAll("movies");

        var countriesContext = new ORMContext<countries>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var countries = countriesContext.ReadAll("countries");

        var genresContext = new ORMContext<genres>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var genres = genresContext.ReadAll("genres");
        
        var user_context = new ORMContext<Users>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var users = user_context.GetAll();
        
        var admin_context = new ORMContext<Admins>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var admins = admin_context.GetAll();
        
        var moviedata_context = new ORMContext<MovieData>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var moviedatas = moviedata_context.ReadAll("films");
        
        dynamic model = new
        {
            Movies = movies,
            Countries = countries,
            Genres = genres,
            Users = users,
            Admins = admins,
            Films = moviedatas
        };
        
        if (!IsAuthorized(Context))
        {
            return Redirect("/admin-login");
        }
        
        var res = template.Render(responseText, model);
        return Html(res);
    }
    
    public bool IsAuthorized(HttpRequestContext context)
    {
        // Проверка наличия Cookie с session-token
        if (context.Request.Cookies.Any(c => c.Name == "admin-session-token"))
        {
            var cookie = context.Request.Cookies["admin-session-token"];
            return SessionStorage.ValidateToken(cookie.Value);
        }

        return false;
    }
    
    [Post("admin/user/add")]
    public IHttpResponseResult AddUser(string addUserLogin, string addUserPassword)
    {
        try
        {
            var user_context = new ORMContext<Users>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            Users newUsers = new Users
            {
                Login = addUserLogin,
                Password = addUserPassword
            };
            user_context.Create(newUsers, "Users");
            var user = user_context.GetUserByLogin(addUserLogin);
            return Json(user);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error adding user: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }
    
    [Post("admin/user/delete")]
    public IHttpResponseResult DeleteUserById(string deleteUserId)
    {
        try
        {
            Console.WriteLine(deleteUserId);
            var user_context = new ORMContext<Users>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            if (deleteUserId != null)
            {
                user_context.Delete(deleteUserId, "Users");
            }
            return Json(user_context.GetAll());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error deleting user: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }
    
    [Post("admin/movie/add")]
    public IHttpResponseResult AddMovie(string addTitle, string addImageUrl, int addYear)
    {
        try
        {
            Console.WriteLine($"Adding movie with title: {addTitle}, image URL: {addImageUrl}, Year: {addYear}");
            var movie_context = new ORMContext<movies>(new SqlConnection(AppConfig.GetInstance().ConnectionString));

            movies newMovie = new movies
            {
                title = addTitle,
                cover_image = addImageUrl,
                Year = addYear
            };
            Console.WriteLine("Attempting to add movie to the database...");
            movie_context.CreateMovie(newMovie);
            Console.WriteLine("Movie added successfully.");

            var movie = movie_context.GetByTitle(addTitle);
            return Json(movie);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error adding movie: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }
    
    [Post("admin/movie/delete")]
    public IHttpResponseResult DeleteMovieById(string deleteMovieId)
    {
        try
        {
            Console.WriteLine(deleteMovieId);
            var movie_context = new ORMContext<movies>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            if (deleteMovieId != null)
            {
                movie_context.DeleteMovie(deleteMovieId, "movies");
            }
            return Json(movie_context.GetAll());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error deleting movie: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }
    
    [Post("admin/moviedata/add")]
    public IHttpResponseResult AddMovieData(string addMovieDataTitle, int addMovieDataYear, string addMovieDataDescription, string addMovieDataQuality, decimal addMovieDataRating, string addMovieDataCountry, int addMovieDuration, string addMovieDataCoverImage, int addMovieDataDirectorId)
    {
        try
        {
            var moviedata_context = new ORMContext<MovieData>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            MovieData newMovieData = new MovieData
            {
                title = addMovieDataTitle,
                year = addMovieDataYear,
                video_quality = addMovieDataQuality,
                kp_rating = addMovieDataRating,
                country = addMovieDataCountry,
                duration = addMovieDuration,
                cover_image = addMovieDataCoverImage,
                plot_summary = addMovieDataDescription,
                director_id = addMovieDataDirectorId
            };
            moviedata_context.CreateMovieData(newMovieData);
            var movieData = moviedata_context.GetByTitle(addMovieDataTitle);
            return Json(movieData);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error adding MovieData: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }
    
    [Post("admin/moviedata/delete")]
    public IHttpResponseResult DeleteMovieDataById(string deleteMovieDataId)
    {
        try
        {
            Console.WriteLine(deleteMovieDataId);
            var moviedata_context = new ORMContext<MovieData>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            if (deleteMovieDataId != null)
            {
                moviedata_context.DeleteMovieData(deleteMovieDataId, "films");
            }
            return Json(moviedata_context.GetAll());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error deleting MovieData: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }
    
    [Post("admin/genre/add")]
    public IHttpResponseResult AddGenre(string addGenreName)
    {
        try
        {
            var genre_context = new ORMContext<genres>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            genres newGenre = new genres()
            {
                name = addGenreName
            };
            genre_context.Create(newGenre, "genres");
            var genre = genre_context.GetByGenreName(addGenreName);
            return Json(genre);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error adding Genre: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }
    
    [Post("admin/genre/delete")]
    public IHttpResponseResult DeleteGenreById(string deleteGenreId)
    {
        try
        {
            Console.WriteLine(deleteGenreId);
            var genre_context = new ORMContext<genres>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            if (deleteGenreId != null)
            {
                genre_context.Delete(deleteGenreId, "genres");
            }
            return Json(genre_context.GetAll());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error deleting Genre: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }
    
    [Post("admin/country/add")]
    public IHttpResponseResult AddCountry(string addCountryName)
    {
        try
        {
            var country_context = new ORMContext<countries>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            countries newCountry = new countries()
            {
                name = addCountryName
            };
            country_context.Create(newCountry, "countries");
            var country = country_context.GetByCountryName(addCountryName);
            return Json(country);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error adding Country: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }
    
    [Post("admin/country/delete")]
    public IHttpResponseResult DeleteCountryById(string deleteCountryId)
    {
        try
        {
            Console.WriteLine(deleteCountryId);
            var country_context = new ORMContext<countries>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            if (deleteCountryId != null)
            {
                country_context.Delete(deleteCountryId, "countries");
            }
            return Json(country_context.GetAll());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error deleting Country: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }
}
