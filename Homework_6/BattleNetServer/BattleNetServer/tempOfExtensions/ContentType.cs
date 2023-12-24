namespace BattleNetServer.tempOfExtensions;

public class ContentType
{
    public static string GetContentType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        switch (extension)
        {
            case ".html":
                return "text/html";
            case ".css":
                return "text/css";
            case ".jpg":
                return "image/jpeg";
            case ".png":
                return "image/png";
            case ".svg":
                return "image/svg+xml";
            default:
                return "text/html";
        }
    }
}