namespace MyHttpServer.Models;

public class MovieData
{
    public int id { get; set; }
    public string title { get; set; }
    public int year { get; set; }
    public string video_quality { get; set; }
    public decimal kp_rating { get; set; }
    public string country { get; set; }
    public int duration { get; set; }
    public string cover_image { get; set; }
    public string plot_summary { get; set; }
    public int director_id { get; set; }
}