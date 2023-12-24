using System.Net;
using BattleNetServer.Configuration;
using Newtonsoft.Json;
using BattleNetServer.Handlers;

namespace BattleNetServer;

public class Server
{
    private static readonly CancellationTokenSource cancellationTokenSource = new();
    private static readonly HttpListener server = new HttpListener();
    private AppSettingsConfig config = Configuration.ServerConfiguration.Config;
    private Handlers.Handler staticFileHandler = new StaticFileHandlers();
    private Handlers.Handler controllerHandler = new ControllerHandler();


    public Server()
    {
        server.Prefixes.Add($"{config.Address}:{config.Port}/");
        Console.WriteLine($"Сервер был запущен по адресу: {config.Address}:{config.Port}/");
    }

    public async Task StartAsync()
    {
        var token = cancellationTokenSource.Token;
        await Task.Run(() => StartServer(token), token);
    }

    private async Task StartServer(CancellationToken token)
    {
        server.Start();
        Task.Run(StopServerOnCommand);

        try
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();

                var context = await server.GetContextAsync();
                staticFileHandler.Successor = controllerHandler;
                staticFileHandler.HandleRequest(context);
            }
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            server.Close();
            ((IDisposable)server).Dispose();
            Console.WriteLine("Сервер завершил свою работу.");        }
    }

    private void StopServerOnCommand()
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (input != null && input.Trim().ToLower() == "stop")
            {
                cancellationTokenSource.Cancel();
                Task.Delay(4000);
                break;
            }
        }
    }
}    