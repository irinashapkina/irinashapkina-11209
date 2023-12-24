using System.Net;
using BattleNetServer.tempOfExtensions;
using BattleNetServer.Configuration;

namespace BattleNetServer.Handlers;

public class StaticFileHandlers : Handler
{
    private AppSettingsConfig config = ServerConfiguration.Config;
        
    public override void HandleRequest(HttpListenerContext context)
    {
        using var response = context.Response;
        var request = context.Request;
        var requestPath = request.Url!.AbsolutePath;
        var pathOfStaticFile = Path.Combine(config.StaticFile, requestPath.Trim('/'));

        
        if (requestPath!.Split('/')!.LastOrDefault()!.Contains('.'))
        {
            var pattern = requestPath?.Split('/')?.LastOrDefault();
            pattern = pattern?[pattern.IndexOf('.')..];
            if (File.Exists(pathOfStaticFile) && pattern != null)
            {
                response.ContentType = ContentType.GetContentType(pattern);
                using var fileStream = File.OpenRead(pathOfStaticFile);
                fileStream.CopyTo(response.OutputStream);   
            }
            else
            {
                using var fileStream = File.OpenRead(Path.Combine(config.StaticFile, "404.html"));
                fileStream.CopyTo(response.OutputStream);
            }
        }
        else if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
    }
}