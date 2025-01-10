using HttpServerLibrary.Configurations;

namespace MyHttpServer.Helpers;

public class ResponseHelper : IResponseHelper
{
    public string GetResponseText(string localPath)
    {
        var filepath = AppConfig.GetInstance().Path + localPath;
        var additionalPath = AppConfig.GetInstance().AddPath + localPath;
        if (File.Exists(filepath))
        {
            var responseText = File.ReadAllText(filepath);
            return responseText;
        }
        else if (File.Exists(additionalPath))
        {
            var responseText = File.ReadAllText(additionalPath);
            return responseText;
        }

        return "error 404";
    }
}