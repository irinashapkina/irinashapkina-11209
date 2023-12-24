using System.Net;
using BattleNetServer;
using BattleNetServer.Configuration;
using Newtonsoft.Json;

class Program
{
    private static async Task Main()
    {
        var server = new Server();
        await server.StartAsync();
    }
}